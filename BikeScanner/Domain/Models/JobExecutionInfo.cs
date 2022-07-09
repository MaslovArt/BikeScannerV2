using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
    public class JobExecutionInfo : EntityBase
	{
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

