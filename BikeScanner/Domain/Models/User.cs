using System;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
	public class User : StatefulCrudBase
	{
        public long UserId { get; set; }
        public string DisplayName { get; set; }
    }
}

