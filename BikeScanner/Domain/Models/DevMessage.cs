using System;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
	public class DevMessage : StatefulCrudBase
	{
        public long UserId { get; set; }
        public string Message { get; set; }
    }
}

