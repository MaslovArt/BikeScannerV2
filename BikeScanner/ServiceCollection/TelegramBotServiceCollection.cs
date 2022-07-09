using BikeScanner.Telegram.Bot;
using BikeScanner.Telegram.Bot.Commands;
using BikeScanner.Telegram.Bot.Commands.DevMessage;
using BikeScanner.Telegram.Bot.Commands.Main;
using BikeScanner.Telegram.Bot.Commands.Search;
using BikeScanner.Telegram.Bot.Commands.Subs;
using BikeScanner.Telegram.Bot.Context;
using BikeScanner.Telegram.HostedServices;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BikeScanner.ServiceCollection
{
    public static class TelegramBotServiceCollection
	{
        public static IServiceCollection AddTelegramBotUI(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.Configure<TelegramAccessConfig>(
                configuration.GetSection(nameof(TelegramAccessConfig)));
            services.Configure<TelegramUIConfig>(
                configuration.GetSection(nameof(TelegramUIConfig)));

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => {
                var bot = x.GetRequiredService<IOptions<TelegramAccessConfig>>().Value;
                return new TelegramBotClient(bot.Key);
            });

            services.AddScoped<IBotContextService, InMemoryBotContext>();
            services.AddSingleton<BikeScannerBot>();

            #region Special commands
            services.AddTransient<ICommandBase, UserBlockBotCommand>();
            services.AddTransient<ICommandBase, UserJoinBotCommand>();
            #endregion
            #region UI Commands
            services.AddTransient<ICommandBase, StartCommand>();
            services.AddTransient<ICommandBase, HelpCommand>();
            services.AddTransient<ICommandBase, SearchCommand>();
            services.AddTransient<ICommandBase, GetSubsCommand>();
            services.AddTransient<ICommandBase, DeleteSubCommand>();
            services.AddTransient<ICommandBase, AddSubCommand>();
            services.AddTransient<ICommandBase, SendMessageCommand>();
            #endregion
            #region Internal commands
            services.AddTransient<ICommandBase, SearchResultsCommand>();
            services.AddTransient<ICommandBase, MoreSearchResultsCommand>();
            services.AddTransient<ICommandBase, ConfirmSubDeleteCommand>();
            services.AddTransient<ICommandBase, ApplySubDeleteCommand>();
            services.AddTransient<ICommandBase, ApplySubAddCommand>();
            services.AddTransient<ICommandBase, ApplySendMessageCommand>();
            #endregion

            services.AddTransient<ICommandBase, UnknownCommand>(); //always last

            return services;
        }

        public static IServiceCollection AddTelegramWebhookHostedService(
            this IServiceCollection services
            )
        {
            services.AddHostedService<TelegramWebhookHostedService>();
            return services;
        }

        public static IServiceCollection AddTelegramPollingHostedService(
            this IServiceCollection services
            )
        {
            services.AddHostedService<TelegramPollHostedService>();
            return services;
        }
    }
}

