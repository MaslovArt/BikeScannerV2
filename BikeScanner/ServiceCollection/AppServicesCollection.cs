using BikeScanner.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class AppServicesCollection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<UsersService>();
            services.AddScoped<SubscriptionsService>();
            services.AddScoped<ContentService>();
            services.AddScoped<DevMessagesService>();
            services.AddScoped<JobExecutionService>();
            services.AddScoped<NotificationService>();

            return services;
        }
    }
}

