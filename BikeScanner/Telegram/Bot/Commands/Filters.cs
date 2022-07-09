using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BikeScanner.Telegram.Bot.Context;
using System.Linq;
using System;

namespace BikeScanner.Telegram.Bot.Commands
{
    /// <summary>
    /// Commands filter delegate
    /// </summary>
    /// <param name="update">Telegram update</param>
    /// <param name="context">Telegram bot context</param>
    /// <returns></returns>
    public delegate bool CommandFilter(Update update, BotContext context);

	public static class CombineFilters
	{
		public static CommandFilter Any(params CommandFilter[] filters) =>
			(u, c) => filters.Any(f => f(u, c));

		public static CommandFilter All(params CommandFilter[] filters) =>
			(u, c) => filters.All(f => f(u, c));
	}

	/// <summary>
	/// Base filters
	/// </summary>
	public static class FilterDefinitions
	{
		/// <summary>
		/// Filter by telegram command name. E.g /start
		/// </summary>
		/// <param name="name">Command name</param>
		/// <exception cref="ArgumentException"></exception>
		/// <returns>Filter</returns>
		public static CommandFilter UICommand(string name)
		{
			if (!name.StartsWith("/"))
				throw new ArgumentException("Command name must start with '/'.");

			return (update, context) =>
				update.Type == UpdateType.Message &&
				update.Message.Type == MessageType.Text &&
				update.Message.Text.ToLower().StartsWith(name);
		}

		/// <summary>
		/// Filter by text command name. E.g Поиск
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Filter</returns>
		public static CommandFilter AlternativeUICommand(string name) =>
			(update, context) =>
				update.Type == UpdateType.Message &&
				update.Message.Type == MessageType.Text &&
				update.Message.Text.Equals(name);

		/// <summary>
		/// Filter by button callback command name
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Filter</returns>
		public static CommandFilter CallbackCommand(string name) =>
			(update, context) =>
				update.Type == UpdateType.CallbackQuery &&
				update.CallbackQuery.Data.StartsWith(name);

		/// <summary>
		/// Filter by state for text messages
		/// </summary>
		/// <param name="state"></param>
		/// <returns>Filter</returns>
		public static CommandFilter StateMessage(BotState state) =>
			(update, context) =>
				update.Type == UpdateType.Message &&
				update.Message.Type == MessageType.Text &&
				context.State == state;

		/// <summary>
        /// Filter for cancel button press
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
		public static CommandFilter Cancel(BotState state) =>
			(update, context) =>
				update.Type == UpdateType.CallbackQuery &&
				update.CallbackQuery.Data.Equals(CommandNames.Internal.Cancel) &&
				context.State == state;

		/// <summary>
		/// User join
		/// </summary>
		/// <returns>Filter</returns>
		public static CommandFilter JoinBot =>
			(update, context) =>
				update.Type == UpdateType.MyChatMember &&
				update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Member;

		/// <summary>
		/// User left
		/// </summary>
		/// <returns>Filter</returns>
		public static CommandFilter LeftBot =>
			(update, context) =>
				update.Type == UpdateType.MyChatMember &&
				update.MyChatMember.NewChatMember.Status == ChatMemberStatus.Kicked;
	}
}

