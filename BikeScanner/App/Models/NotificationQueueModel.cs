namespace BikeScanner.App.Models
{
    public class NotificationQueueModel
	{
        public long UserId { get; set; }
        public string Text { get; set; }

        public NotificationQueueModel()
        { }

        public NotificationQueueModel(long userId, string text)
        {
            UserId = userId;
            Text = text;
        }
    }
}

