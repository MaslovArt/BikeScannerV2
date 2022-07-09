using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BikeScanner.Telegram.Bot.Commands
{
    public abstract class CommandBase : ICommandBase
    {
        public abstract Task Execute(CommandContext context);

        public abstract CommandFilter Filter { get; }

        /// <summary>
        /// Base params separator symbol
        /// </summary>
        protected char ParamSeparator => ';';

        /// <summary>
        /// Get input as param
        /// </summary>
        /// <param name="context"></param>
        /// <param name="param">Param index</param>
        /// <param name="exclude">Exclude command name from input</param>
        /// <returns></returns>
        protected string GetParam(
            CommandContext context,
            int param,
            params string[] excludeCommands
            )
        {
            var input = ChatInput(context);
            if (excludeCommands != null)
            {
                foreach (var excludeCommand in excludeCommands)
                    input = input.Replace(excludeCommand, "");
            }

            var paramsInput = input.Trim();

            return paramsInput.Split(ParamSeparator)[param];
        }

        /// <summary>
        /// User input from message or callback
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Message or callback text or empty string</returns>
        protected string ChatInput(CommandContext context)
        {
            var input = context.Update.Message?.Text
                ?? context.Update.CallbackQuery?.Data
                ?? string.Empty;

            return input.Trim();
        }

        /// <summary>
        /// User input from message or callback
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Message or callback text or empty string</returns>
        protected string ChatInput(CommandContext context, params string[] excludeCommands)
        {
            var input = context.Update.Message?.Text
                ?? context.Update.CallbackQuery?.Data
                ?? string.Empty;

            if (excludeCommands != null)
            {
                foreach (var excludeCommand in excludeCommands)
                    input = input.Replace(excludeCommand, "");
            } 

            return input.Trim();
        }

        protected bool IsCallback(CommandContext context)
        {
            return context.Update.Type == UpdateType.CallbackQuery;
        }

        protected Task EditCallbackMessage(CommandContext context, InlineKeyboardMarkup markup)
        {
            if (context.Update.CallbackQuery is null)
                throw new ArgumentException("CallbackQuery update required!");

            return context.Client.EditMessageTextAsync(
                context.Update.CallbackQuery.Message.Chat,
                context.Update.CallbackQuery.Message.MessageId,
                context.Update.CallbackQuery.Message.Text,
                replyMarkup: markup
                );
        }

        protected Task DeleteMessage(CommandContext context)
        {
            if (context.Update.CallbackQuery is null)
                throw new ArgumentException("CallbackQuery update required!");

            return context.Client.DeleteMessageAsync(
                context.Update.CallbackQuery.Message.Chat,
                context.Update.CallbackQuery.Message.MessageId
                );
        }

        protected Task EditCallbackMessage(string text, CommandContext context, InlineKeyboardMarkup markup)
        {
            if (context.Update.CallbackQuery is null)
                throw new ArgumentException("CallbackQuery update required!");

            return context.Client.EditMessageTextAsync(
                context.Update.CallbackQuery.Message.Chat,
                context.Update.CallbackQuery.Message.MessageId,
                text,
                replyMarkup: markup
                );
        }

        protected Task AnswerCallback(string text, CommandContext context)
        {
            var callbackId = context.Update.CallbackQuery.Id;
            return context.Client.AnswerCallbackQueryAsync(callbackId, text, false);
        }

        /// <summary>
        /// Send messages (possible random order)
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected Task SendMessages(IEnumerable<string> messages, CommandContext context)
        {
            var tasks = messages.Select(m => SendMessage(m, context));
            return Task.WhenAll(tasks);
        }

        protected Task SendMessage(string message, CommandContext context) =>
            context.Client.SendTextMessageAsync(context.UserId, message);

        protected Task SendMessage(
            string message,
            CommandContext context,
            IReplyMarkup markup
            ) =>
            context.Client.SendTextMessageAsync(context.UserId, message, replyMarkup: markup);
    }
}

