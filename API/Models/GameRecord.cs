using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class GameRecord(string result, int playerChoice, int computerChoice)
    {
        [JsonPropertyName("results")]
        public string Result { get; init; } = result;

        [JsonPropertyName("player")]
        public int PlayerChoice { get; init; } = playerChoice;

        [JsonPropertyName("computer")]
        public int ComputerChoice { get; init; } = computerChoice;
    }
}
