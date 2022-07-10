using System;
using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class PostModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }

        [JsonPropertyName("is_pinned")]
        public int PinnedValue { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("date")]
        public long DateStamp { get; set; }

        public bool IsPinned => PinnedValue == 1;

        public string Url => $"https://vk.com/{(OwnerId > 0 ? $"id{OwnerId}" : $"club{Math.Abs(OwnerId)}")}?w=wall{OwnerId}_{Id}";
    }
}

