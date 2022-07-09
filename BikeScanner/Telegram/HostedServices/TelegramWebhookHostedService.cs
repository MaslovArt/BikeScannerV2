using System;
using System.Threading;
using System.Threading.Tasks;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BikeScanner.Telegram.HostedServices
{
    public class TelegramWebhookHostedService : IHostedService
    {
        private readonly ILogger<TelegramWebhookHostedService> _logger;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly TelegramAccessConfig _botConfig;
        private readonly string _webhookAddress;

        public TelegramWebhookHostedService(
            ILogger<TelegramWebhookHostedService> logger,
            ITelegramBotClient telegramBotClient,
            IOptions<TelegramAccessConfig> options)
        {
            _logger = logger;
            _telegramBotClient = telegramBotClient;
            _botConfig = options.Value;
            _webhookAddress = $"{_botConfig.Webhook}/bot/{_botConfig.Key}";

            if (string.IsNullOrEmpty(_botConfig.Webhook))
                throw new ArgumentException("No webhook configuration!");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _telegramBotClient.SetWebhookAsync(
                url: _webhookAddress,
                allowedUpdates: new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery,
                    UpdateType.MyChatMember
                },
                cancellationToken: cancellationToken);
            _logger.LogInformation($"Add webhook: {_webhookAddress}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
            _logger.LogInformation($"Delete webhook: {_webhookAddress}");
        }
    }
}

