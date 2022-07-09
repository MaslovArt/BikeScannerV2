using System;
using Telegram.Bot.Types;

namespace BikeScanner.Core.Exceptions
{
    public class UpdateUserIdException : Exception
    {
        public UpdateUserIdException(Update update)
            : base($"Can't get chat id for update {update.Type}")
        { }
    }
}

