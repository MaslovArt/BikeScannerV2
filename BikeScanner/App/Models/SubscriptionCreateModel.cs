namespace BikeScanner.App.Models
{
    public class SubscriptionCreateModel
	{
        public long UserId { get; set; }
        public string SearchQuery { get; set; }

        public SubscriptionCreateModel()
        { }

        public SubscriptionCreateModel(long userId, string searchQuery)
        {
            UserId = userId;
            SearchQuery = searchQuery;
        }
    }
}

