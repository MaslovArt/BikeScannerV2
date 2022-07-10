using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;
using Telegram.Bot.Types.Enums;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Add new subscription. Save user input to subs.
    /// </summary>
    public class ApplySubAddCommand : CommandBase
    {
        private readonly ContentService          _searchService;
        private readonly SubscriptionsService   _subsService;

        public ApplySubAddCommand(
            SubscriptionsService subsService,
            ContentService searchService
            )
        {
            _subsService = subsService;
            _searchService = searchService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.StateMessage(BotState.WaitNewSubInput),
            FilterDefinitions.CallbackCommand(CommandNames.Internal.AddSubFromSearch)
            );

        public override async Task Execute(CommandContext context)
        {
            var searchQuery = ChatInput(context, CommandNames.Internal.AddSubFromSearch);

            var newSub = new SubscriptionCreateModel(context.UserId, searchQuery);
            await _subsService.CreateAsync(newSub);

            var message = $"\nПоиск '{searchQuery}' сохранен в подписках.";
            if (context.Update.Type == UpdateType.CallbackQuery)
            {
                await AnswerCallback($"Поиск '{searchQuery}' cохранен", context);
                await EditCallbackMessage(context, null);
            }
            else
            {
                await SendMessage(message, context);
                await TryFindAdsByNewSub(searchQuery, context);
            }

            context.BotContext.State = BotState.Default;
        }

        private async Task TryFindAdsByNewSub(string searchQuery, CommandContext context)
        {
            var count = await _searchService.CountSearch(searchQuery);
            if (count > 0)
            {
                var message = $"По новой подписке '{searchQuery}' уже есть {count} объявлений.";
                var showBtn = TelegramMarkupHelper.MessageRowBtns(
                    ("Посмотреть", $"{CommandNames.Internal.ShowSubsFromSearch} {searchQuery}")
                    );

                await SendMessage(message, context, showBtn);
            }
        }
    }
}

