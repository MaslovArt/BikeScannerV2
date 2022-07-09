using System.Threading.Tasks;
using BikeScanner.App.Services;

namespace BikeScanner.Telegram.Bot.Commands.Main
{
    /// <summary>
    /// User stop bot
    /// </summary>
    public class UserBlockBotCommand : CommandBase
    {
        private readonly UsersService _usersService;

        public UserBlockBotCommand(UsersService usersService)
        {
            _usersService = usersService;
        }

        public override CommandFilter Filter => FilterDefinitions.LeftBot;

        public override Task Execute(CommandContext context) =>
            _usersService.DiactivateUser(context.UserId);
    }
}

