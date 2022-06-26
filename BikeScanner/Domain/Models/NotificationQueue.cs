using System;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
	public class NotificationQueue : StatefulCrudBase
	{
        public long UserId { get; set; }
        public string Text { get; set; }
    }
}

