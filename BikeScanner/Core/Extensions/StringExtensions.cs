using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeScanner.Core.Extensions
{
    public static class StringExtensions
    {
        public static string[] WithMaxBytes(this IEnumerable<string> array, int bytesCount)
        {
            return array.Select(x => x.WithMaxBytes(bytesCount)).ToArray();
        }

        public static string WithMaxBytes(this string value, int bytesCount)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length > bytesCount)
            {
                return Encoding.UTF8.GetString(bytes.Take(bytesCount).ToArray());
            }
            return value;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string str) =>
            string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        public static bool IsMinLength(this string str, int minLength) =>
            !str.IsNullOrEmptyOrWhiteSpace() && str.Length >= minLength;

        public static string ReplaceAll(this string str, IEnumerable<string> oldStrs, string newStr)
        {
            foreach (var oldStr in oldStrs)
                str = str.Replace(oldStr, newStr);
            return str;
        }

    }
}

