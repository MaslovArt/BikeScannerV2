using System.Threading.Tasks;
using BikeScanner.App.Jobs.Base;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Crawling => AutoSearch => Notify
    /// </summary>
    public class ScannerJob : JobBase
	{
		private readonly AdditionalCrawlingJob	_additionalCrawlingJob;
		private readonly AutoSearchJob			_autoSearchJob;
		private readonly NotificationsSenderJob	_notificationsSenderJob;

        public override string JobName => "Scanner";

        public ScannerJob(
			AdditionalCrawlingJob additionalCrawlingJob,
			AutoSearchJob autoSearchJob,
			NotificationsSenderJob crawlSearchNotifyJob,
			ILogger<ScannerJob> logger
			)
			: base(logger)
		{
			_additionalCrawlingJob = additionalCrawlingJob;
			_autoSearchJob = autoSearchJob;
			_notificationsSenderJob = crawlSearchNotifyJob;
		}

        public override async Task Run()
        {
            await _additionalCrawlingJob.Execute(performContext);
            await _autoSearchJob.Execute(performContext);
            await _notificationsSenderJob.Execute(performContext);
        }

    }
}

