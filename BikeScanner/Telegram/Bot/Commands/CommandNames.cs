namespace BikeScanner.Telegram.Bot.Commands
{
    /// <summary>
    /// Commands names
    /// </summary>
	public static class CommandNames
	{
        /// <summary>
        /// Main telegram commands. Call from chat input (like /start)
        /// </summary>
        public static class UI
        {
            public const string Start = "/start";
            public const string Help = "/help";
            public const string Search = "/search";
            public const string MySubs = "/my_subs";
            public const string DeleteSub = "/delete_sub";
            public const string AddSub = "/add_sub";
            public const string DevMessage = "/dev_message";
        }

        /// <summary>
        /// Alternative commands. Call from chat input (readable for users)
        /// </summary>
        public static class AlternativeUI
        {
            public const string Search = "Поиск";
            public const string MySubs = "Подписки";
            public const string Help = "Помощь";
        }

        /// <summary>
        /// Internal commands. Call from buttons
        /// </summary>
        public static class Internal
        {
            public const string AddSubFromSearch = "sub_from_search";
            public const string ShowSubsFromSearch = "show_subs_from_search";
            public const string MoreSearchResults = "more_search_results";
            public const string ConfirmDeleteSub = "confirm_delete_sub";
            public const string ApplyDeleteSub = "apply_delete_sub";
            public const string Cancel = "cancel";
        }
    }
}

