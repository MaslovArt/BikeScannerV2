using Telegram.Bot;
using Telegram.Bot.Types;
using BikeScanner.Telegram.Bot.Context;
using BikeScanner.Core.Exceptions;

namespace BikeScanner.Telegram.Bot.Commands
{
	public class CommandContext
	{
        public BotContext BotContext { get; private set; }
        public Update Update { get; private set; }
        public ITelegramBotClient Client { get; private set; }

        public CommandContext(
            Update update,
            ITelegramBotClient client,
            BotContext botContext
            )
        {
            Update = update;
            Client = client;
            BotContext = botContext;
        }

        public long UserId =>
            Update.Message?.Chat?.Id ??
            Update.CallbackQuery?.Message?.Chat?.Id ??
            Update.MyChatMember?.Chat?.Id ??
            throw new UpdateUserIdException(Update);

        public string UserName => "";
    }
}

