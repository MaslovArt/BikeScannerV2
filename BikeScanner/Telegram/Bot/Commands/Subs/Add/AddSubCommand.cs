using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Add new subscription. Ask what to add.
    /// </summary>
    public class AddSubCommand : CommandUIBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.AddSub),
            FilterDefinitions.CallbackCommand(CommandNames.UI.AddSub)
            );

        public override Task ExecuteCommand(CommandContext context)
        {
            context.BotContext.State = BotState.WaitNewSubInput;

            var questionMsg = @"Какой поиск добавить в подписку?
Отправлю уведомление, когда появится похожее объявление.";
            return SendMessage(questionMsg, context);
        }
    }
}

