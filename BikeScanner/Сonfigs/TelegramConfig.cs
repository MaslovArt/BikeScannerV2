namespace BikeScanner.Сonfigs
{
    public class TelegramAccessConfig
	{
        public string Key { get; set; }
        public string BotName { get; set; }
        public string Webhook { get; set; }
    }

    public class TelegramUIConfig
    {
        public int SearchItemsPerPage = 5;
    }
}

