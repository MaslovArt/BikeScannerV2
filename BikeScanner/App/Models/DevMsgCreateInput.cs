namespace BikeScanner.App.Models
{
    public class DevMsgCreateInput
	{
        public long UserId { get; set; }
        public string Message { get; set; }

        public DevMsgCreateInput()
        { }

        public DevMsgCreateInput(long userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}

