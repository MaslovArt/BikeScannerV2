using Microsoft.Extensions.Configuration;

namespace BikeScanner.Core.Extensions
{
    public static class ConfigurationExtensions
	{
		public static string DefaultConnection(this IConfiguration configuration) =>
			configuration.GetConnectionString("DefaultConnection");
	}
}

