using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Ask what to delete.
    /// </summary>
    public class DeleteSubCommand : CommandUIBase
    {
        private readonly SubscriptionsService _subsService;

        public DeleteSubCommand(SubscriptionsService subscriptionsService)
        {
            _subsService = subscriptionsService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.DeleteSub),
            FilterDefinitions.CallbackCommand(CommandNames.UI.DeleteSub));

        public override async Task ExecuteCommand(CommandContext context)
        {
            var userSubs = await _subsService.GetUserSubs<ViewSubscriptionOutput>(context.UserId);
            if (userSubs.Length == 0)
            {
                await SendMessage("Удалять нечего. Подписок нет.", context);
                return;
            }

            var btns = userSubs
                .Select(s => (s.SearchQuery, $"{CommandNames.Internal.ConfirmDeleteSub} {s.Id}"))
                .Append(BaseButtons.Cancel)
                .ToArray();
            await EditCallbackMessage("Какой удалить?", context, TelegramMarkupHelper.MessageColumnBtns(btns));

            context.BotContext.State = BotState.DeleteSub;
        }
    }
}

