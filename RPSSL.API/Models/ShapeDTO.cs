using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class ShapeDTO
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
