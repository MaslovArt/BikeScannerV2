using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class VkArray<T>
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("items")]
        public T[] Items { get; set; }
    }
}

