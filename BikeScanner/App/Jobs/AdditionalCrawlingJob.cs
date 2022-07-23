using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.App.Jobs.Base;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Add new contents to local storage
    /// </summary>
    public class AdditionalCrawlingJob : JobBase
    {
        private record ContentUrl(string Url);

        private readonly JobExecutionService            _jobExecutionService;
        private readonly ContentService                 _contentService;
        private readonly ICrawler[]                     _crawlers;
        private readonly BikeScannerConfig              _config;

        public override string JobName => "Additional content crawling";

        public AdditionalCrawlingJob(
            ILogger<AdditionalCrawlingJob> logger,
            IEnumerable<ICrawler> crawlers,
            IOptions<BikeScannerConfig> options,
            JobExecutionService jobExecutionService,
            ContentService contentService
            )
            : base(logger)
        {
            _crawlers = crawlers.ToArray();
            _config = options.Value;
            _jobExecutionService = jobExecutionService;
            _contentService = contentService;
        }

        public override async Task Run()
        {
            var defaultTime = DateTime.Now.AddDays(-_config.ContentLifeTime);
            var lastAdditionalCrawlingExecTime = await _jobExecutionService
                .GetLastCrawlingTime() ?? defaultTime;
            var urls = (await _contentService.GetAll<ContentUrl>())
                .Select(u => u.Url);

            bool crawlWithError = false;
            var tasks = _crawlers.Select(l =>
            {
                try
                {
                    return l.Get(lastAdditionalCrawlingExecTime);
                }
                catch (Exception ex)
                {
                    LogCritical(ex.Message, ex);
                    crawlWithError = true;
                    return Task.FromResult(Array.Empty<ContentModel>());
                }
            });
            var tasksResults = await Task.WhenAll(tasks);

            var content = tasksResults
                .SelectMany(r => r)
                .Where(c => !urls.Contains(c.Url))
                .ToList();

            var sourceCounts = content
                .GroupBy(c => c.SourceType)
                .Select(g => $"{g.Key}:{g.Count()}");
            LogInformation($"Crawl {content.Count} new items ({string.Join(",", sourceCounts)}).");

            await _contentService.CreateManyAsync(content);
            await _jobExecutionService.SetLastCrawlingTime(DateTime.Now);

            if (crawlWithError )
                throw new Exception("Some crawlers completed with error");
        }

    }
}

