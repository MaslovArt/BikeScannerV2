using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    public class CommentModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("from_id")]
        public int UserId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}

