using System;
using BikeScanner.DAL;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.ServiceCollection
{
	public static class EFServiceCollectionExtensions
	{
        public static IServiceCollection AddPostgresDB(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddDbContext<BikeScannerContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}

