using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class AlbumModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }
}

