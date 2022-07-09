using System.Threading.Tasks;
using BikeScanner.App.Services;
using BikeScanner.Telegram.Bot.Helpers;

namespace BikeScanner.Telegram.Bot.Commands.Subs
{
    /// <summary>
    /// Delete sub. Delete selected sub.
    /// </summary>
	public class ApplySubDeleteCommand : GetSubsCommand
	{
        public ApplySubDeleteCommand(SubscriptionsService subscriptionsService)
            : base(subscriptionsService)
        { }

        public override CommandFilter Filter =>
            FilterDefinitions.CallbackCommand(CommandNames.Internal.ApplyDeleteSub);

        public override async Task Execute(CommandContext context)
        {
            var subId = int.Parse(ChatInput(context, CommandNames.Internal.ApplyDeleteSub));
            await _subsService.DeleteLogicalAsync(subId);

            var deleteMessage = $"{Emoji.X} Поиск удален. {Emoji.X}";
            await AnswerCallback(deleteMessage, context);

            await base.Execute(context);
        }
    }
}

