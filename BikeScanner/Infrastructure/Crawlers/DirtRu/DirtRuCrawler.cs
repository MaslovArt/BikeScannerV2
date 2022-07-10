using System;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.App.Models;
using BikeScanner.Infrastructure.Crawlers.DirtRu.Models;
using BikeScanner.Infrastructure.Crawlers.DirtRu.Services;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.Infrastructure.Crawlers.DirtRu
{
    /// <summary>
    /// Download ads from dirt.ru market
    /// </summary>
    public class DirtRuCrawler : ICrawler
    {
        private readonly ILogger<DirtRuCrawler> _logger;
        private readonly DirtRuSourceConfig _sourceConfig;
        private readonly DirtRuParser _dirtRuParser;

        public DirtRuCrawler(
            ILogger<DirtRuCrawler> logger,
            IOptions<DirtRuSourceConfig> sourceOptions)
        {
            _logger = logger;
            _sourceConfig = sourceOptions.Value;
            _dirtRuParser = new DirtRuParser(_logger);
        }

        public async Task<ContentModel[]> Get(DateTime since)
        {
            var pageForums = await _dirtRuParser.GetForumsInfo();
            _logger.LogInformation($"Get {pageForums.Length} forums from page ({string.Join(',', pageForums.Select(f => f.ForumId))})");

            var source = GetActualSource(pageForums, since);
            var excudeKeys = _sourceConfig.ExcludeKeys ?? Array.Empty<string>();
            var getTasks = source.Select(s => _dirtRuParser.GetForumItems(since, s, excudeKeys, _sourceConfig.MaximumParsePages));
            var items = await Task.WhenAll(getTasks);

            return items
                .SelectMany(i => i)
                .Select(i => new ContentModel()
                {
                    Url = i.Url,
                    Text = $"{i.Prefix} {i.Text}",
                    Published = i.Published,
                    SourceType = CrawlerType.DirtRu.ToString(),
                })
                .ToArray();
        }

        private DirtRuForumConfig[] GetActualSource(ForumInfo[] pageForums, DateTime since)
        {
            var source = _sourceConfig.Forums ?? Array.Empty<DirtRuForumConfig>()
                .Where(s => pageForums.Any(f => f.ForumId == s.ForumId))
                .ToArray();
            if (source.Length != _sourceConfig.Forums.Length)
            {
                var notFoundForums = _sourceConfig.Forums.Except(source);
                _logger.LogWarning($"Forums not found: {string.Join(',', notFoundForums.Select(f => f.ForumId))}");
            }

            source = source
                .Where(s => pageForums.First(f => f.ForumId == s.ForumId).ForumUpdate > since)
                .ToArray();
            _logger.LogInformation($"Check {source.Length} forums updates ({string.Join(',', source.Select(f => f.ForumId))})");

            return source;
        }
    }
}

