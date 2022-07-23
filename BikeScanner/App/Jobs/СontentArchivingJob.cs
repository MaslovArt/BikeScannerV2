using System;
using System.Threading.Tasks;
using BikeScanner.App.Jobs.Base;
using BikeScanner.App.Services;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Move outdated contents to archive
    /// </summary>
    public class СontentArchivingJob : JobBase
    {
        private readonly ContentService _contentService;
        private readonly DateTime       _archiveSince;

        public СontentArchivingJob(
            ILogger<СontentArchivingJob> logger,
            ContentService contentService,
            IOptions<BikeScannerConfig> options
            )
            : base(logger)
		{
            _contentService = contentService;
            _archiveSince = DateTime.Now
                .AddDays(-options.Value.ContentLifeTime)
                .Date;
		}

        public override string JobName => "Contents arhiving";

        public override async Task Run()
        {
            var count = await _contentService.ArchiveContents(_archiveSince);
            LogInformation($"Archived {count} items older than {_archiveSince.ToShortDateString()}");
        }
    }
}

