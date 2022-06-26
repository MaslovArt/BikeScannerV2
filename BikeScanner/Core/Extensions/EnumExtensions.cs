using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BikeScanner.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnumByDescription<T>(this string description, T defaultValue)
        {
            MemberInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (T)Enum.Parse(typeof(T), fi.Name);
            }

            return defaultValue;
        }

        public static string[] GetDescriptions<T>(params T[] exclude)
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Where(e => exclude != null ? !exclude.Contains(e) : true)
                .Select(e => e.GetDescription())
                .ToArray();
        }
    }
}

