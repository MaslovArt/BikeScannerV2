using System.Text;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Show user subscriptions
    /// </summary>
    public class GetSubsCommand : CommandUIBase
    {
        protected readonly SubscriptionsService _subsService;

        public GetSubsCommand(SubscriptionsService subsService)
        {
            _subsService = subsService;
        }

        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.MySubs),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.MySubs),
            FilterDefinitions.Cancel(BotState.DeleteSub)
            );

        public override async Task ExecuteCommand(CommandContext context)
        {
            var subs = await _subsService.GetUserSubs<ViewSubscriptionOutput>(context.UserId);

            if (subs.Length == 0)
            {
                var addSubBtn = TelegramMarkupHelper.MessageRowBtns(
                    ("Добавить подписку", CommandNames.UI.AddSub));
                await SendMessage("Нет подписок", context, addSubBtn);
                return;
            }

            var message = new StringBuilder($"Всего подписок: {subs.Length}\n\n");
            foreach (var sub in subs)
                message.AppendLine($"• {sub.SearchQuery}");
            var btns = TelegramMarkupHelper.MessageRowBtns(
                ("Добавить", CommandNames.UI.AddSub),
                ("Удалить", CommandNames.UI.DeleteSub)
                );

            if (IsCallback(context))
                await EditCallbackMessage(message.ToString(), context, btns);
            else
                await SendMessage(message.ToString(), context, btns);
        }
    }
}

