using System.Net;

namespace Mmicovic.RPSSL.API.Exceptions
{
    internal class HttpNumberGenerationUnavailableException()
        : HttpException(HttpStatusCode.ServiceUnavailable)
    {
        public override string Message { get => "Random number generation is temporarily unavailable"; }
    }
}
