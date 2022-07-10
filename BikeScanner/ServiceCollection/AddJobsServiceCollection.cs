using BikeScanner.App.Jobs;
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
            services.AddTransient<CrawlSearchNotifyJob>();

            return services;
        }
    }
}

