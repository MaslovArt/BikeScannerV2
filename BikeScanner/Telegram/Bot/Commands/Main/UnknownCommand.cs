using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.Main
{
    /// <summary>
    /// Handle unknown user input
    /// </summary>
    public class UnknownCommand : CommandBase
    {
        public override CommandFilter Filter => (_, _) => true;

        public override Task Execute(CommandContext context)
        {
            var wtfMessage = @$"Хм..
Я не знаю что с этим делать(
Тут можно посмотреть что я могу ({CommandNames.UI.Help})";
            return SendMessage(wtfMessage, context);
        }
    }
}

