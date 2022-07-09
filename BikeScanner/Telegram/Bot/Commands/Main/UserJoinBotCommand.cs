using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands.Main
{
    /// <summary>
    /// User join bot
    /// </summary>
    public class UserJoinBotCommand : CommandBase
    {
        //private readonly IUsersService _usersService;

        //public UserJoinBotCommand(IUsersService usersService)
        //{
        //    _usersService = usersService;
        //}

        public override CommandFilter Filter => FilterDefinitions.JoinBot;

        public override Task Execute(CommandContext context) => Task.CompletedTask;
        //{
        //    var userId = UserId(context);
        //    var user = await _usersService.EnsureUser(userId);

        //    if (user.State == AccountState.Inactive)
        //    {
        //        await _usersService.ActivateUser(userId);
        //    }
        //}
    }
}

