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
    /// Search. Show search results.
    /// </summary>
    public class SearchResultsCommand : CommandBase
    {
        private readonly int _perPage;
        private readonly ContentService _searchService;

        public SearchResultsCommand(
            ContentService searchService,
            IOptions<BikeScannerConfig> options
            )
        {
            _perPage = options.Value.TelegramSearchItemsPerPage;
            _searchService = searchService;
        }

        public override CommandFilter Filter =>
            CombineFilters.Any(
                FilterDefinitions.StateMessage(BotState.WaitSearchInput),
                FilterDefinitions.CallbackCommand(CommandNames.Internal.ShowSubsFromSearch)
                );

        public override async Task Execute(CommandContext context)
        {
            var searchQuery = ChatInput(context, CommandNames.Internal.ShowSubsFromSearch);

            var result = await _searchService.Search<ViewContentModel>(searchQuery, 0, _perPage);

            var resultMessage = $"По запросу '{searchQuery}' нашел {result.Total} объявлений.";
            var saveSearchBtn = TelegramMarkupHelper.MessageRowBtns(
                ("Сохранить поиск", $"{CommandNames.Internal.AddSubFromSearch} {searchQuery}"));
            await SendMessage(resultMessage, context, saveSearchBtn);

            var adUrls = result.Items.Select(r => r.Url);
            await SendMessages(adUrls, context);

            if (result.Total > result.Items.Length)
            {
                var moreMessage = $"Показать еще? ({result.Total - result.Items.Length})";
                var moreButton = TelegramMarkupHelper.MessageRowBtns(
                    ("Еще", $"{CommandNames.Internal.MoreSearchResults} {searchQuery}{ParamSeparator}{_perPage}"));
                await SendMessage(moreMessage, context, moreButton);
            }

            context.BotContext.State = BotState.Default;
        }
    }
}

