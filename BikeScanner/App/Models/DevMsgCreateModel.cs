namespace BikeScanner.App.Models
{
    public class DevMsgCreateModel
	{
        public long UserId { get; set; }
        public string Message { get; set; }

        public DevMsgCreateModel()
        { }

        public DevMsgCreateModel(long userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}

