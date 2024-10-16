using System.Text.Json;
using System.Text.Json.Serialization;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.Models
{
    public class SnakeCaseJsonStringEnumConverter : JsonStringEnumConverter
    {
        public SnakeCaseJsonStringEnumConverter() : base(JsonNamingPolicy.SnakeCaseLower) { }
    }

    public class GameRecord(long? id, Result? result, int? playerChoice, int? computerChoice)
    {
        public GameRecord(Service.Models.GameRecord serviceObject)
            : this(serviceObject.Id, serviceObject.Result, serviceObject.PlayerChoice, serviceObject.ComputerChoice)
        { }

        public GameRecord() : this(null, null, null, null) { }

        [JsonPropertyName("id")]
        public long? Id { get; init; } = id;

        [JsonPropertyName("results")]
        [JsonConverter(typeof(SnakeCaseJsonStringEnumConverter))]
        public Result? Result { get; init; } = result;

        [JsonPropertyName("player")]
        public int? PlayerChoice { get; init; } = playerChoice;

        [JsonPropertyName("computer")]
        public int? ComputerChoice { get; init; } = computerChoice;

        public Service.Models.GameRecord ToServiceObject(string user)
        {
            return new Service.Models.GameRecord(user, Result, PlayerChoice, ComputerChoice);
        }
    }
}
