using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Polly;
using Polly.Retry;

namespace Mmicovic.RPSSL.Service
{
    public interface IRandomGenerator
    {
        Task<int> Next(int minValue, int maxValue, CancellationToken ct);
    }

    public class NumberGenerationUnavailableException : Exception { };

    public class ExternalRandomGenerator(IConfiguration configuration, ILogger<ExternalRandomGenerator> logger)
        : IRandomGenerator
    {
        private static readonly int MAX_RETRY_ATTEMPTS = 4;
        private static readonly TimeSpan GENERATION_DELAY = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan GENERATION_TIMEOUT = TimeSpan.FromSeconds(10);

        private const string RANDOM_NUMBER_FIELD = "random_number";
        private const string EXTERNAL_GENERATOR_URL_CONFIG = "ExternalRandomGeneratorUrl";

        private readonly HttpClient httpClient = new();
        private readonly IConfiguration configuration = configuration;
        private readonly ILogger<ExternalRandomGenerator> logger = logger;

        public async Task<int> Next(int minValue, int maxValue, CancellationToken ct)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("maxValue must be strictly greater than minValue");

            // Set a global timeout for all retries
            httpClient.Timeout = GENERATION_TIMEOUT;

            try
            {
                // The external API could experience temporary issues
                // To mitigate that we try a total of MAX_RETRY_ATTEMPTS+1 times, with a delay of GENERATION_DELAY
                var pipeline = BuildRetryPipeline(MAX_RETRY_ATTEMPTS, GENERATION_DELAY);
                return await pipeline.ExecuteAsync(async token =>
                {
                    var response = await SendRequest(token);
                    var randomNumber = ParseResponse(response);

                    // The response needs to be mapped to our desired [minValue, maxValue) range
                    return TransposeResponse(randomNumber, minValue, maxValue);
                }, ct);
            }
            catch (Exception ex)
            {
                if (ct.IsCancellationRequested && ex is TaskCanceledException)
                {
                    logger.LogDebug("The call has been cancelled");
                    throw;
                }

                // The stack trace is lost here, so we log the error first
                logger.LogError(ex, ex.Message);
                throw new NumberGenerationUnavailableException();
            }
        }

        private static ResiliencePipeline BuildRetryPipeline(int maxRetries, TimeSpan delay)
        {
            var retryOptions = new RetryStrategyOptions
            {
                Delay = delay,
                MaxRetryAttempts = maxRetries
            };
            return new ResiliencePipelineBuilder()
                    .AddRetry(retryOptions)
                    .Build();
        }

        private async Task<string> SendRequest(CancellationToken token)
        {
            var externalGeneratorUrl = configuration.GetConnectionString(EXTERNAL_GENERATOR_URL_CONFIG);

            logger.LogDebug($"Sending random number request to: {externalGeneratorUrl}...");
            var responseMessage = await httpClient.GetAsync(externalGeneratorUrl, token);
            responseMessage.EnsureSuccessStatusCode();

            return await responseMessage.Content.ReadAsStringAsync(token);
        }

        private int ParseResponse(string response)
        {
            logger.LogDebug($"Parsing external random number response: {response}...");
            var jsonResponse = JsonNode.Parse(response);
            return jsonResponse![RANDOM_NUMBER_FIELD]!.GetValue<int>();
        }

        private int TransposeResponse(int random, int minValue, int maxValue)
        {
            logger.LogDebug($"Transposing external random number: {random}...");

            // First transpose to [0, maxValue-minValue) using the modulo,
            // Then add minValue so that the result is transposed to [minValue, maxValue)
            var modulus = maxValue - minValue;
            random = (random % modulus) + minValue;

            logger.LogDebug($"Transposed external random number: {random}");
            return random;
        }
    }
}
