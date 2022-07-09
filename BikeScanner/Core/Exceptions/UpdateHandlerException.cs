using System;
using BikeScanner.Telegram.Bot.Context;
using Telegram.Bot.Types;

namespace BikeScanner.Core.Exceptions
{
    public class UpdateHandlerException : Exception
    {
        public UpdateHandlerException(Update update, BotContext context)
            : base($"No handler for update: [{update.Type}] context: [{context.State}]")
        { }
    }
}

