using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BikeScanner.App.Services;
using BikeScanner.Сonfigs;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.App.Jobs
{
    public class СontentArchivingJob
	{
        private readonly ILogger<СontentArchivingJob>   _logger;
        private readonly ContentService                 _contentService;
        private readonly DateTime                       _archiveSince;

        public СontentArchivingJob(
            ILogger<СontentArchivingJob> logger,
            ContentService contentService,
            IOptions<BikeScannerConfig> options
            )
		{
            _logger = logger;
            _contentService = contentService;
            _archiveSince = DateTime.Now
                .AddDays(-options.Value.ContentLifeTime)
                .Date;
		}

        [JobDisplayName("Content archiving")]
        public async Task Execute()
        {
            _logger.LogInformation($"Starting archive contents older than {_archiveSince.ToShortDateString()}.");

            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var count = await _contentService.ArchiveContents(_archiveSince);
                _logger.LogInformation($"Archived {count} items in {watch.Elapsed}.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Archive error: {ex.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Finish archive in {watch.Elapsed}");
            }
        }

    }
}

