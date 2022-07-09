using System;
using System.Threading;
using System.Threading.Tasks;
using BikeScanner.Telegram.Bot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BikeScanner.Telegram.HostedServices
{
    internal class TelegramPollHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<TelegramPollHostedService> _logger;
        private readonly CancellationTokenSource _cts;
        private readonly BikeScannerBot _bot;

        public TelegramPollHostedService(
            ITelegramBotClient telegramBotClient,
            ILogger<TelegramPollHostedService> logger,
            BikeScannerBot bot
            )
        {
            _botClient = telegramBotClient;
            _logger = logger;
            _cts = new CancellationTokenSource();
            _bot = bot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var info = await _botClient.GetWebhookInfoAsync(cancellationToken);
            if (!string.IsNullOrEmpty(info.Url))
            {
                await _botClient.DeleteWebhookAsync(true, cancellationToken);
                _logger.LogInformation($"Delete webhook {info.Url} before polling");
            }

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions()
                {
                    AllowedUpdates = new UpdateType[]
                    {
                        UpdateType.Message,
                        UpdateType.CallbackQuery,
                        UpdateType.MyChatMember
                    }
                },
                _cts.Token);
            _logger.LogInformation($"{nameof(TelegramPollHostedService)}: Start redirect polling to webhook.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation($"{nameof(TelegramPollHostedService)}: Stop.");

            return Task.CompletedTask;
        }

        private Task HandleErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken cancellationToken
            )
        {
            _logger.LogError(exception, exception.Message);

            return Task.CompletedTask;
        }

        public Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
            )
        {
            return _bot.Handle(update);
        }
    }
}

