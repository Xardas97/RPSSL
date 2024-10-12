using System.Text.Json;
using System.Text.Json.Serialization;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Models
{
    public class SnakeCaseJsonStringEnumConverter : JsonStringEnumConverter
    {
        public SnakeCaseJsonStringEnumConverter() : base(JsonNamingPolicy.SnakeCaseLower) { }
    }

    public class GameRecord(Service.Models.GameRecord serviceObject)
    {
        [JsonPropertyName("results")]
        [JsonConverter(typeof(SnakeCaseJsonStringEnumConverter))]
        public Result Result { get; init; } = serviceObject.Result;

        [JsonPropertyName("player")]
        public int PlayerChoice { get; init; } = serviceObject.Player1Choice;

        [JsonPropertyName("computer")]
        public int ComputerChoice { get; init; } = serviceObject.Player2Choice;
    }
}
