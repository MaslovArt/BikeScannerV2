using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;

namespace BikeScanner.Telegram.Bot.Commands.DevMessage
{
    /// <summary>
    /// Send message to admin. Receive user input.
    /// </summary>
    public class ApplySendMessageCommand : CommandBase
    {
        private readonly DevMessagesService _devMessages;

        public ApplySendMessageCommand(DevMessagesService devMessagesService)
        {
            _devMessages = devMessagesService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.StateMessage(BotState.WaitDevMessageInput);

        public override async Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.Default;

            var input = ChatInput(context);
            var newMsg = new DevMsgCreateInput(context.UserId, input);
            await _devMessages.CreateAsync(newMsg);

            await SendMessage("Сообщение отправлено.", context);
        }
    }
}

