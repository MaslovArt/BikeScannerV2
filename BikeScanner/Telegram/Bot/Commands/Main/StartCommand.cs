using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using BikeScanner.Domain.States;
using BikeScanner.Telegram.Bot.Helpers;
using static BikeScanner.Telegram.Bot.Commands.CommandNames;

namespace BikeScanner.Telegram.Bot.Commands.Main
{
    /// <summary>
    /// Start bot
    /// </summary>
    public class StartCommand : HelpCommand
	{
        private readonly UsersService _usersService;

        public StartCommand(UsersService usersService)
        {
            _usersService = usersService;
        }

        public override CommandFilter Filter =>
            FilterDefinitions.UICommand(CommandNames.UI.Start);

        public async override Task ExecuteCommand(CommandContext context)
        {
            var userModel = new UserCreateModel()
            {
                DisplayName = context.UserName,
                UserId = context.UserId
            };
            var user = await _usersService.EnsureUser(userModel);

            if (user.IsInState(UserStates.Stopped))
                await _usersService.ActivateUser(context.UserId);

            var message = @$"Привет!
Я бот для поиска по объявлениям.

Список доступных команд:
{CommandNames.UI.Search} - Поиск
{CommandNames.UI.MySubs} - Мои подписки
{CommandNames.UI.AddSub} - Добавить подписку
{CommandNames.UI.DeleteSub} - Удалить подписку
{CommandNames.UI.DevMessage} - Сообщение админу
{CommandNames.UI.Help} - Помощь
{CommandNames.UI.Start} - Перезапуск бота";

            var buttons = TelegramMarkupHelper.KeyboardRowBtns(
                AlternativeUI.Search,
                AlternativeUI.MySubs
                );

            await SendMessage(message, context, buttons);
            await base.ExecuteCommand(context);
        }
    }
}

