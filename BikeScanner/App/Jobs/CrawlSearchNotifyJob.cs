using System.Threading.Tasks;
using Hangfire;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Crawling => AutoSearch => Notify
    /// </summary>
    public class CrawlSearchNotifyJob
	{
		private readonly AdditionalCrawlingJob	_additionalCrawlingJob;
		private readonly AutoSearchJob			_autoSearchJob;
		private readonly NotificationsSenderJob	_notificationsSenderJob;

		public CrawlSearchNotifyJob(
			AdditionalCrawlingJob additionalCrawlingJob,
			AutoSearchJob autoSearchJob,
			NotificationsSenderJob crawlSearchNotifyJob
			)
		{
			_additionalCrawlingJob = additionalCrawlingJob;
			_autoSearchJob = autoSearchJob;
			_notificationsSenderJob = crawlSearchNotifyJob;
		}

        [JobDisplayName("Additional crawling, autoserch and notify")]
        public async Task Execute()
        {
			await _additionalCrawlingJob.Execute();
			await _autoSearchJob.Execute();
			await _notificationsSenderJob.Execute();
        }
	}
}

