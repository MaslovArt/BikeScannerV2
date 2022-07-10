using System;
using BikeScanner.App.Jobs;
using BikeScanner.Сonfigs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BikeScanner.App.Hangfire
{
	public static class JobsSetup
	{
		public static void ConfigeJobs(IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(serviceProvider));
            GlobalJobFilters.Filters
                .Add(new AutomaticRetryAttribute { Attempts = 0 });

            var cronExpression = serviceProvider
                .GetRequiredService<IOptions<BikeScannerConfig>>()
                .Value.CrawlJobCron;

            RecurringJob.AddOrUpdate<AdditionalCrawlingJob>(
                JobNames.ADDITIONAL_CRAWLING,
                j => j.Execute(),
                cronExpression
                );
            RecurringJob.AddOrUpdate<NotificationsSenderJob>(
                JobNames.NOTIFICATIONS,
                j => j.Execute(),
                Cron.Never
                );
            RecurringJob.AddOrUpdate<AutoSearchJob>(
                JobNames.AUTO_SEARCH,
                j => j.Execute(),
                Cron.Never
                );
            RecurringJob.AddOrUpdate<CrawlSearchNotifyJob>(
                JobNames.CRAWL_SEARCH_NOTIFY,
                j => j.Execute(),
                Cron.Never
                );
        }
	}
}

