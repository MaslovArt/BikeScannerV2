using System.Collections.Generic;
using System.Linq;
using BikeScanner.Core.Extensions;
using Telegram.Bot.Types.ReplyMarkups;

namespace BikeScanner.Telegram.Bot.Helpers
{
    /// <summary>
    /// Telegram markup generation
    /// </summary>
	public static class TelegramMarkupHelper
	{
        /// <summary>
        /// Return direction row buttons for keyboard
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <returns></returns>
        public static ReplyKeyboardMarkup KeyboardRowBtns(params string[] btns)
        {
            var buttons = new List<List<KeyboardButton>>
            {
                btns.Select(o => new KeyboardButton(o)).ToList()
            };
            return new ReplyKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction row buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <returns></returns>
		public static InlineKeyboardMarkup MessageRowBtns(params string[] btns)
        {
            var buttons = btns
                .WithMaxBytes(TelegramConsts.MaxButtonByteLen)
                .Select(btn => InlineKeyboardButton.WithCallbackData(btn));
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction row buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (btn text, btn callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageRowBtns(params (string text, string callback)[] btns)
        {
            var buttons = btns
                .Select(btn => InlineKeyboardButton.WithCallbackData(
                    btn.text.WithMaxBytes(TelegramConsts.MaxButtonByteLen),
                    btn.callback.WithMaxBytes(TelegramConsts.MaxButtonByteLen))
                );
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction column buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageColumnBtns(params string[] btns)
        {
            var buttons = btns
                .WithMaxBytes(TelegramConsts.MaxButtonByteLen)
                .Select(btn => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(btn)
                });
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction column buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (btn text, btn callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageColumnBtns(params (string text, string callback)[] btns)
        {
            var buttons = btns
                .Select(btn => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(
                        btn.text.WithMaxBytes(TelegramConsts.MaxButtonByteLen),
                        btn.callback.WithMaxBytes(TelegramConsts.MaxButtonByteLen))
                });
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return link button for message
        /// </summary>
        /// <param name="url">Link url</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageViewLink(string url)
        {
            var link = InlineKeyboardButton.WithUrl("Посмотреть", url);
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>() { link });
        }
    }
}
