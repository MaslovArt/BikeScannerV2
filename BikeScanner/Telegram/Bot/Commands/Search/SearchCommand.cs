using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.Search
{
    /// <summary>
    /// Search. Ask what to search.
    /// </summary>
    public class SearchCommand : CommandUIBase
    {
        public override CommandFilter Filter => CombineFilters.Any(
            FilterDefinitions.UICommand(CommandNames.UI.Search),
            FilterDefinitions.AlternativeUICommand(CommandNames.AlternativeUI.Search)
            );

        public override Task ExecuteCommand(CommandContext context)
        {
            context.BotContext.State = BotState.WaitSearchInput;
            return SendMessage("Что найти?", context);
        }
    }
}

