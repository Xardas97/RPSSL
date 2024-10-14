using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class PlayCommand(int? shapeId)
    {
        [JsonPropertyName("player")]
        public int? ShapeId { get; init; } = shapeId;
    }
}
