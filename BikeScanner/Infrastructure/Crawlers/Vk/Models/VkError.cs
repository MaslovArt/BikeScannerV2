using System.Text.Json.Serialization;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class VkError
    {
        [JsonPropertyName("error_code")]
        public int Code { get; set; }

        [JsonPropertyName("error_msg")]
        public string Message { get; set; }

        public override string ToString()
        {
            return $"[{Code}] {Message}";
        }
    }
}

