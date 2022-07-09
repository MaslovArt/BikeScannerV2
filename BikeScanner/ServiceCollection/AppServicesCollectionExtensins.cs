using BikeScanner.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class AppServicesCollectionExtensins
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<UsersService>();
            services.AddScoped<SubscriptionsService>();
            services.AddScoped<SearchService>();
            services.AddScoped<DevMessagesService>();
            services.AddScoped<JobExecutionService>();

            return services;
        }
    }
}

