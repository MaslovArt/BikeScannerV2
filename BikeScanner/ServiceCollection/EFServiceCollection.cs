using System;
using BikeScanner.Core.Extensions;
using BikeScanner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class EFServiceCollection
	{
        public static IServiceCollection AddPostgresDB(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            services.AddDbContext<BikeScannerContext>(options =>
            {
                options.UseNpgsql(configuration.DefaultConnection());
            });

            return services;
        }
    }
}

