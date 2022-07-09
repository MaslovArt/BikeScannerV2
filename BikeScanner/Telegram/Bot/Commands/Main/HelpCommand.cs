using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.Main
{
    /// <summary>
    /// Explain how to use the bot
    /// </summary>
    public class HelpCommand : CommandUIBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.Help),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.Help)
            );

        public override Task ExecuteCommand(CommandContext context)
        {
            var helpMessage = @$"
Для поиска по объявлением нужно запустить комманду ({CommandNames.UI.Search}), дальше думаю разберетесь)
Если желаемого найти не удалось, можно добавить подписку на поиск ({CommandNames.UI.AddSub}).
Как только появится похожее объявление, я сообщу.
Подписка больше неактуальна - удаляй ее ({CommandNames.UI.DeleteSub}).

Хочешь сообщить об ошибке в работе бота, предложить улучшения или сказать что это полная хрень и послать автора - {CommandNames.UI.DevMessage}.";

            return SendMessage(helpMessage, context);
        }
    }
}

