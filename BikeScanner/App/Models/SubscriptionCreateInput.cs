namespace BikeScanner.App.Models
{
    public class SubscriptionCreateInput
	{
        public long UserId { get; set; }
        public string SearchQuery { get; set; }

        public SubscriptionCreateInput()
        { }

        public SubscriptionCreateInput(long userId, string searchQuery)
        {
            UserId = userId;
            SearchQuery = searchQuery;
        }
    }
}

