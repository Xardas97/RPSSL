using System.Text.Json.Serialization;

namespace Mmicovic.RPSSL.API.Models
{
    public class Shape(Service.Models.Shape serviceObject)
    {
        [JsonPropertyName("id")]
        public int Id { get; init; } = serviceObject.Id;

        [JsonPropertyName("name")]
        public string Name { get; init; } = serviceObject.Name;
    }
}
