using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands
{
	public abstract class CommandUIBase : CommandBase
	{
        public abstract Task ExecuteCommand(CommandContext context);

        public override Task Execute(CommandContext context)
        {
            context.BotContext.State = BotState.Default;

            return ExecuteCommand(context);
        }
    }
}

