using Microsoft.Extensions.Configuration;

namespace BikeScanner.Core.Extensions
{
    public static class ConfigurationExtensions
	{
		public static string DefaultConnection(this IConfiguration configuration) =>
			configuration.GetConnectionString("DefaultConnection");

        public static T GetSectionAs<T>(this IConfiguration config, string key) =>
            config.GetSection(key).Get<T>();
    }

}

