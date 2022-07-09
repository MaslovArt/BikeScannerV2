using BikeScanner.Telegram.Bot.Commands;

namespace BikeScanner.Telegram.Bot.Helpers
{
	public static class BaseButtons
	{
		public static (string, string) Cancel =>
			($"{Emoji.ArrowLeft} Отмена", CommandNames.Internal.Cancel);
	}
}

