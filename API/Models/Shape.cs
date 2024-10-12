using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class Shape(int id, string name)
    {
        [JsonPropertyName("id")]
        public int Id { get; init; } = id;

        [JsonPropertyName("name")]
        public string Name { get; init; } = name;
    }
}
