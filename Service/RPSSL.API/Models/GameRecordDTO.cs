using System.Text.Json;
using System.Text.Json.Serialization;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Models
{
    public class SnakeCaseJsonStringEnumConverter : JsonStringEnumConverter
    {
        public SnakeCaseJsonStringEnumConverter() : base(JsonNamingPolicy.SnakeCaseLower) { }
    }

    public class GameRecordDTO(long? id, Result? result, int? playerChoice, int? computerChoice)
    {
        public GameRecordDTO() : this(null, null, null, null) { }

        [JsonPropertyName("id")]
        public long? Id { get; init; } = id;

        [JsonPropertyName("results")]
        [JsonConverter(typeof(SnakeCaseJsonStringEnumConverter))]
        public Result? Result { get; init; } = result;

        [JsonPropertyName("player")]
        public int? PlayerChoice { get; init; } = playerChoice;

        [JsonPropertyName("computer")]
        public int? ComputerChoice { get; init; } = computerChoice;
    }
}
