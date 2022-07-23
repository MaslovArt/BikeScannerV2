using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BikeScanner.Core.Extensions;
using BikeScanner.Infrastructure.Crawlers.DirtRu.Models;
using BikeScanner.Сonfigs;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Crawlers.DirtRu.Services
{
    internal class DirtRuParser
    {
        private readonly ILogger _logger;

        public DirtRuParser(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ForumInfo[]> GetForumsInfo()
        {
            var marketUrl = $"https://forum.dirt.ru/forumdisplay.php?f=7";
            var document = await DownloadPage(marketUrl);

            var forumTables = new List<HtmlNode>();
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_22"));
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_23"));
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_23 ~ tbody"));

            if (forumTables.Count == 0)
                _logger.LogWarning("Parse market page: No forums found!");

            return forumTables
                .SelectMany(table => table.GetChildElements())
                .Select(TryParseForumInfoRow)
                .Where(f => f != null)
                .ToArray();
        }

        public async Task<ForumItem[]> GetForumItems(
            DateTime since,
            DirtRuForumConfig source,
            string[] excludeWith,
            int maximumParsePage)
        {
            var items = new List<ForumItem>();

            var currentPage = 1;
            while (currentPage < maximumParsePage)
            {
                var specificForumUrl = $"https://forum.dirt.ru/forumdisplay.php?f={source.ForumId}&order=desc&page={currentPage++}";
                var document = await DownloadPage(specificForumUrl);

                var forumTableId = $"#threadbits_forum_{source.ForumId}";
                var forumItemsRows = document.QuerySelectorAll($"{forumTableId} tr");

                var forumItems = forumItemsRows
                    .Select(TryParseForumItemRow)
                    .Where(i => i != null)
                    .Where(i => !excludeWith
                        .Any(k => i.Text.ToUpper().Contains(k.ToUpper())))
                    .ToArray();

                var newItems = forumItems
                    .Where(i => i.Published > since)
                    .ToArray();
                items.AddRange(newItems);

                if (newItems.Length != forumItems.Length) break;
            }

            _logger.LogInformation($"[{source.ForumName}:{source.ForumId}] get {items.Count} items from {currentPage - 1} pages");

            return items.ToArray();
        }

        private async Task<HtmlNode> DownloadPage(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(pageContents);

            return document.DocumentNode;
        }

        private ForumItem TryParseForumItemRow(HtmlNode node)
        {
            try
            {
                var columns = node.GetChildElements().ToArray();
                var itemId = columns[2]
                    .Attributes["id"].Value
                    .Replace("td_title_", "");
                var prefix = columns[1].InnerText;
                var desctiption = columns[2]
                    .QuerySelector($"#thread_title_{itemId}")
                    .InnerText;
                var published = columns[3].InnerText
                    .ReplaceAll(new string[] { "\r", "\n", "\t" }, "")
                    .Split("от")[0];
                var authorIdContainer = columns[2]
                    .GetChildElements()
                    .ToArray()[1]
                    .InnerHtml;
                var authorId = int.Parse(Regex.Matches(authorIdContainer, @"(?<=u=)[0-9]+").First().Value);

                return new ForumItem()
                {
                    Url = $"https://forum.dirt.ru/showthread.php?t={itemId}",
                    Text = desctiption,
                    Prefix = prefix,
                    Published = DateTime.ParseExact(published, "dd.MM.yyyy HH:mm", null),
                    AuthorId = authorId
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Can't parse forum item row [html: {node.InnerHtml}]");
                return null;
            }
        }

        private ForumInfo TryParseForumInfoRow(HtmlNode node)
        {
            try
            {
                var forumIdStr = node
                    .QuerySelector(".alt1Active a")
                    .Attributes["href"]
                    .Value.Split('=').Last();
                var updateStr = node
                    .QuerySelector(".alt2 .smallfont div:nth-child(3)")
                    .InnerText
                    .ReplaceAll(new string[] { "\r", "\n", "\t" }, "");

                return new ForumInfo()
                {
                    ForumId = int.Parse(forumIdStr),
                    ForumUpdate = DateTime.ParseExact(updateStr, "dd.MM.yyyy HH:mm", null)
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Can't parse forum info row [html: {node.InnerHtml}]");
                return null;
            }
        }
    }
}

