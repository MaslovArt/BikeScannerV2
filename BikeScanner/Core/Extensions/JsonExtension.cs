using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BikeScanner.Core.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T obj)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            return JsonSerializer.Serialize(obj, options);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}

