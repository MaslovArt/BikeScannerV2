using System.Threading.Tasks;

namespace BikeScanner.Telegram.Bot.Commands
{
    /// <summary>
    /// Telegram update handler
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// Execute command logic
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task Execute(CommandContext context);

        /// <summary>
        /// Update filter: what update this command can handle.
        /// </summary>
        CommandFilter Filter { get; }
    }
}