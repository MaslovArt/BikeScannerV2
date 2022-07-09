using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Options;

namespace BikeScanner.Telegram.Bot.Commands.Search
{
    /// <summary>
    /// Search. Show next page search results.
    /// </summary>
    public class MoreSearchResultsCommand : CommandBase
    {
        private readonly int _perPage;
        private readonly SearchService _searchService;

        public MoreSearchResultsCommand(
            SearchService searchService,
            IOptions<TelegramUIConfig> options
            )
        {
            _perPage = options.Value.SearchItemsPerPage;
            _searchService = searchService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.MoreSearchResults);

        public override async Task Execute(CommandContext context)
        {
            var searchQuery = GetParam(context, 0, CommandNames.Internal.MoreSearchResults);
            var skip = int.Parse(GetParam(context, 1, CommandNames.Internal.MoreSearchResults));

            var result = await _searchService.Search<ViewContentOutput>(searchQuery, skip, _perPage);

            var adUrls = result.Items.Select(r => r.Url);
            await SendMessages(adUrls, context);
            await DeleteMessage(context);

            if (result.Total > result.Offset)
            {
                var moreMessage = $"Показать еще? ({result.Total - result.Offset})";
                var moreButton = TelegramMarkupHelper.MessageRowBtns(
                    ("Еще", $"{CommandNames.Internal.MoreSearchResults} {searchQuery}{ParamSeparator}{skip + _perPage}"));
                await SendMessage(moreMessage, context, moreButton);
            }
        }
    }
}

