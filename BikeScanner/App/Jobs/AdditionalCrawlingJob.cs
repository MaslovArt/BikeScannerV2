using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Сonfigs;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.App.Jobs
{
    public class AdditionalCrawlingJob
    {
        private record ContentUrl(string Url);

        private readonly JobExecutionService            _jobExecutionService;
        private readonly ContentService                 _contentService;
        private readonly ILogger<AdditionalCrawlingJob> _logger;
        private readonly ICrawler[]                     _crawlers;
        private readonly BikeScannerConfig              _config;

        public AdditionalCrawlingJob(
            ILogger<AdditionalCrawlingJob> logger,
            IEnumerable<ICrawler> crawlers,
            IOptions<BikeScannerConfig> options,
            JobExecutionService jobExecutionService,
            ContentService contentService
            )
        {
            _logger = logger;
            _crawlers = crawlers.ToArray();
            _config = options.Value;
            _jobExecutionService = jobExecutionService;
            _contentService = contentService;
        }

        [JobDisplayName("Additional content crawling")]
        public async Task Execute()
        {
            var lastAdditionalCrawlingExecTime = await _jobExecutionService
                .GetLastCrawlingTime() ?? DateTime.Now.AddDays(-_config.ContentLifeTime);

            var crawlingWatch = new Stopwatch();
            crawlingWatch.Start();

            _logger.LogInformation($"Starting crawling ({_crawlers.Length} providers)");

            try
            {
                var urls = (await _contentService.GetAll<ContentUrl>())
                    .Select(u => u.Url);
                var content = (await GetContent(lastAdditionalCrawlingExecTime))
                    .Where(c => !urls.Contains(c.Url))
                    .ToList();
                content.ForEach(c => c.Published = c.Published.ToUniversalTime());

                await _contentService.CreateManyAsync(content);
                await _jobExecutionService.SetLastCrawlingTime(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Crawling error: {ex.Message}");
                throw;
            }
            finally
            {
                crawlingWatch.Stop();
                _logger.LogInformation($"Finish crawling in {crawlingWatch.Elapsed}");
            }
        }

        private async Task<ContentModel[]> GetContent(DateTime since)
        {
            var timer = new Stopwatch();

            timer.Start();
            var tasks = _crawlers.Select(l => l.Get(since));
            var tasksResults = await Task.WhenAll(tasks);
            timer.Stop();

            var results = tasksResults
                .SelectMany(r => r)
                .ToArray();

            var downloadTime = timer.Elapsed;
            _logger.LogInformation($"Download {results.Length} items in {downloadTime}");

            return results;
        }
    }
}

