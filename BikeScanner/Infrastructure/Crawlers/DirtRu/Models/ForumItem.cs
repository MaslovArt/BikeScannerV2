using System;

namespace BikeScanner.Infrastructure.Crawlers.DirtRu.Models
{
    public class ForumItem
    {
        public string Prefix { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime Published { get; set; }
        public int AuthorId { get; set; }
    }
}

