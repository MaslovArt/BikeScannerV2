using System;
using BikeScanner.App.Jobs;
using BikeScanner.Core.Extensions;
using BikeScanner.Сonfigs;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace BikeScanner.App.Hangfire
{
    public static class JobsSetup
	{
		public static void ConfigeJobs(
            IServiceProvider serviceProvider,
            IConfiguration configuration
            )
        {
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(serviceProvider));
            GlobalJobFilters.Filters
                .Add(new AutomaticRetryAttribute { Attempts = 0 });

            var jobConfig = configuration
                .GetSectionAs<JobConfig>(nameof(JobConfig));

            RecurringJob.AddOrUpdate<AdditionalCrawlingJob>(
                JobNames.ADDITIONAL_CRAWLING,
                j => j.Execute(null),
                Cron.Never
                );
            RecurringJob.AddOrUpdate<NotificationsSenderJob>(
                JobNames.NOTIFICATIONS,
                j => j.Execute(null),
                Cron.Never
                );
            RecurringJob.AddOrUpdate<AutoSearchJob>(
                JobNames.AUTO_SEARCH,
                j => j.Execute(null),
                Cron.Never
                );

            RecurringJob.AddOrUpdate<ScannerJob>(
                JobNames.SCANNER,
                j => j.Execute(null),
                jobConfig.ScannerJobCron
                );

            RecurringJob.AddOrUpdate<СontentArchivingJob>(
                JobNames.CONTENT_ARCHIVING,
                j => j.Execute(null),
                jobConfig.ContentArchivingJobCron
                );

        }
	}
}

