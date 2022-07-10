using System;
namespace BikeScanner.Сonfigs
{
    public class DirtRuForumConfig
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; }
    }

    public class DirtRuSourceConfig
    {
        public int MaximumParsePages { get; set; }
        public string[] ExcludeKeys { get; set; }
        public DirtRuForumConfig[] Forums { get; set; }
    }
}

