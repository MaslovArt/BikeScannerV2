using BikeScanner.App.Jobs;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class AddJobsServiceCollection
    {
        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddTransient<AdditionalCrawlingJob>();
            services.AddTransient<AutoSearchJob>();
            services.AddTransient<NotificationsSenderJob>();
            services.AddTransient<ScannerJob>();
            services.AddTransient<СontentArchivingJob>();

            return services;
        }
    }
}

