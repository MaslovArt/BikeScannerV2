using System.Text.Json.Serialization;
using BikeScanner.Infrastructure.Api;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Models
{
    internal class VkResponse<T> : IApiResult
    {
        [JsonPropertyName("error")]
        public VkError Error { get; set; }

        [JsonPropertyName("response")]
        public T Response { get; set; }

        public bool IsSuccess => Error == null;

        public string ErrorMessage => Error?.Message;

        public int? ErrorCode => Error?.Code;
    }
}

