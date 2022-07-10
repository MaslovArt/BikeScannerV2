using System;
using System.Threading.Tasks;
using BikeScanner.Infrastructure.Api;
using BikeScanner.Infrastructure.Crawlers.Vk.Api.Methods;
using BikeScanner.Infrastructure.Crawlers.Vk.Models;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Api
{
    /// <summary>
    /// Vk Api
    /// </summary>
    internal class VkApi
    {
        private readonly VkAccessConfig _settings;
        private readonly ApiManager _api;

        public VkApi(ILogger logger, VkAccessConfig settings)
        {
            if (settings.MaxApiRequestsPerSecond < 0)
                throw new ArgumentException($"{nameof(settings.MaxApiRequestsPerSecond)} min value is 1!");

            _settings = settings;
            _api = new ApiManager(logger, settings.MaxApiRequestsPerSecond);
        }

        /// <summary>
        /// Get albums
        /// </summary>
        /// <param name="ownerId">User/group id</param>
        /// <param name="albumsIds">Albums ids</param>
        /// <returns>Albums or empty array</returns>
        public async Task<AlbumModel[]> GetAlbums(int ownerId, int[] albumsIds = null)
        {
            var getAlbumsMethod = new GetAlbumsMethod(_settings)
            {
                OwnerId = ownerId,
                AlbumsIds = albumsIds
            };
            var result = await _api.Request<VkResponse<VkArray<AlbumModel>>>(getAlbumsMethod);

            return result.IsSuccess
                ? result.Response.Items
                : Array.Empty<AlbumModel>();
        }

        /// <summary>
        /// Get wall posts
        /// </summary>
        /// <param name="ownerId">User/group id</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>Posts or empty array</returns>
        public async Task<PostModel[]> GetWallPosts(int ownerId, int offset, int count)
        {
            var getWallPostsMethod = new GetPostsMethod(_settings)
            {
                OwnerId = ownerId,
                Offset = offset,
                Count = count
            };
            var result = await _api.Request<VkResponse<VkArray<PostModel>>>(getWallPostsMethod);

            return result.IsSuccess
                ? result.Response.Items
                : Array.Empty<PostModel>();
        }

        /// <summary>
        /// Get photos
        /// </summary>
        /// <param name="ownerId">User/group id</param>
        /// <param name="albumId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>Photos or empty array</returns>
        public async Task<PhotoModel[]> GetPhotos(int ownerId, int albumId, int offset, int count)
        {
            var getPhotosMethod = new GetPhotosMethod(_settings)
            {
                OwnerId = ownerId,
                AlbumId = albumId,
                Offset = offset,
                Count = count,
                DateDesc = true
            };
            var result = await _api.Request<VkResponse<VkArray<PhotoModel>>>(getPhotosMethod);

            return result.IsSuccess
                ? result.Response.Items
                : Array.Empty<PhotoModel>();
        }
    }
}

