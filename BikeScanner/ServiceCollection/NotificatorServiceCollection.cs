using BikeScanner.App.Interfaces;
using BikeScanner.Infrastructure.Notificators;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class NotificatorServiceCollection
	{
        public static IServiceCollection AddTelegramNotificator(this IServiceCollection services)
        {
            services.AddSingleton<INotificator, TelegramNotificator>();

            return services;
        }

        public static IServiceCollection AddLogNotificator(this IServiceCollection services)
        {
            services.AddSingleton<INotificator, LogNotificator>();

            return services;
        }
    }
}

