using System;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.Domain.Models
{
	public class Content : StatefulCrudBase
	{
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public string SourceType { get; set; }
        public string SourceId { get; set; }
        public int AuthorId { get; set; }
    }
}

