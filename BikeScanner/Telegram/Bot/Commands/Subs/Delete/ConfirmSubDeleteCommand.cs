using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Confirm user input.
    /// </summary>
    public class ConfirmSubDeleteCommand : CommandBase
    {
        private readonly SubscriptionsService _subsService;

        public ConfirmSubDeleteCommand(SubscriptionsService subscriptionsService)
        {
            _subsService = subscriptionsService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.ConfirmDeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var subId = int.Parse(ChatInput(context, CommandNames.Internal.ConfirmDeleteSub));
            var sub = await _subsService.GetRecordAsync<ViewSubscriptionModel>(subId);

            var confirmMessage = $"Подтвердите удаление '{sub.SearchQuery}'";
            var confirmBtn = TelegramMarkupHelper.MessageColumnBtns(
                ($"{Emoji.X} Удалить", $"{CommandNames.Internal.ApplyDeleteSub} {sub.Id}"),
                BaseButtons.Cancel);
            await EditCallbackMessage(confirmMessage, context, confirmBtn);
        }
    }
}
