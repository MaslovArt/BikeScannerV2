using System;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
	public class Subscription : StatefulCrudBase
	{
        public long UserId { get; set; }
        public string SearchQuery { get; set; }
    }
}

