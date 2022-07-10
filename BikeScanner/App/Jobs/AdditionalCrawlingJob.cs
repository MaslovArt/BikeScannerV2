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
            _logger.LogInformation($"Starting crawling ({_crawlers.Length} providers)");

            var lastAdditionalCrawlingExecTime = await _jobExecutionService
                .GetLastCrawlingTime() ?? DateTime.Now.AddDays(-_config.ContentLifeTime);

            var crawlingWatch = new Stopwatch();
            crawlingWatch.Start();
            try
            {
                var urls = (await _contentService.GetAll<ContentUrl>())
                    .Select(u => u.Url);

                Exception lastCrawlException = null;
                var tasks = _crawlers.Select(l =>
                {
                    try
                    {
                        return l.Get(lastAdditionalCrawlingExecTime);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, ex.Message);
                        lastCrawlException = ex;
                        return Task.FromResult(Array.Empty<ContentModel>());
                    }
                });
                var tasksResults = await Task.WhenAll(tasks);

                var content = tasksResults
                    .SelectMany(r => r)
                    .Where(c => !urls.Contains(c.Url))
                    .ToList();
                content.ForEach(c => c.Published = c.Published.ToUniversalTime());

                await _contentService.CreateManyAsync(content);
                await _jobExecutionService.SetLastCrawlingTime(DateTime.Now);

                if (lastCrawlException != null) throw lastCrawlException;
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

    }
}

