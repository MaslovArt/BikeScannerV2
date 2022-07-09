using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.DevMessage
{
    /// <summary>
    /// Send message to admin. Ask what to send.
    /// </summary>
    public class SendMessageCommand : CommandUIBase
    {
        public override CommandFilter Filter =>
            FilterDefinitions.UICommand(CommandNames.UI.DevMessage);

        public override Task ExecuteCommand(CommandContext context)
        {
            context.BotContext.State = BotState.WaitDevMessageInput;

            var question = @"Какое сообщение отправить разработчику?";
            return SendMessage(question, context);
        }
    }
}

