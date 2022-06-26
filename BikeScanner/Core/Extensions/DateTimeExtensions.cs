using System;

namespace BikeScanner.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long UnixStamp(this DateTime date) =>
            ((DateTimeOffset)date).ToUnixTimeSeconds();
    }
}

