using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class PhotoModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }

        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }

        [JsonPropertyName("user_id")]
        public int AuthorId { get; set; }

        [JsonPropertyName("date")]
        public long DateStamp { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        public string Url => $"https://vk.com/albums{OwnerId}?z=photo{OwnerId}_{Id}";
    }
}

