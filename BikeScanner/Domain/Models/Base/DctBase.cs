using System;
namespace BikeScanner.Domain.Models.Base
{
	public class DctBase : EntityBase
	{
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

