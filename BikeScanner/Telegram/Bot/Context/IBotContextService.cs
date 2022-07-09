using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Context
{
    public interface IBotContextService
	{
		Task<BotContext> GetUserContext(long userId);
		Task<BotContext> EnsureContext(long userId);
		Task Update(BotContext context);
	}
}
