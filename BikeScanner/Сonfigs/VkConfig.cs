namespace BikeScanner.Сonfigs
{
    public class VkAccessConfig
    {
        public string ApiKey { get; set; }

        public string Version { get; set; }

        public int MaxApiRequestsPerSecond { get; set; }
    }

    public class VkSourseConfig
    {
        public WallSourceConfig[] Walls { get; set; }
        public AlbumSourceConfig[] Albums { get; set; }
        public int MaxPostsPerGroup { get; set; }
    }

    public class WallSourceConfig
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
    }

    public class AlbumItemConfig
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
    }

    public class AlbumSourceConfig : WallSourceConfig
    {
        public AlbumItemConfig[] List { get; set; }
    }

}

