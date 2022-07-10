using BikeScanner.Сonfigs;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Api.Methods
{
    /// <summary>
    /// Get user/group albums
    /// </summary>
    internal class GetAlbumsMethod : VkApiMethod
    {
        public GetAlbumsMethod(VkAccessConfig settings)
            : base(settings)
        { }

        public override string Method => "photos.getAlbums";

        public override string Params => $"owner_id={OwnerId}&album_ids={string.Join(',', AlbumsIds)}";

        public int OwnerId { get; set; }

        public int[] AlbumsIds { get; set; }
    }
}

