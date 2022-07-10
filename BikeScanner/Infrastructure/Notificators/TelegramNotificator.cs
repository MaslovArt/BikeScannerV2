using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using Telegram.Bot;

namespace BikeScanner.Infrastructure.Notificators
{
    public class TelegramNotificator : INotificator
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramNotificator(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public Task Send(long userId, string message) =>
            _telegramBotClient.SendTextMessageAsync(userId, message);
    }
}

