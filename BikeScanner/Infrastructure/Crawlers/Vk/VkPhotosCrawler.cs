using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.App.Models;
using BikeScanner.Core.Extensions;
using BikeScanner.Infrastructure.Crawlers.Vk.Api;
using BikeScanner.Infrastructure.Crawlers.Vk.Models;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.Infrastructure.Crawlers.Vk
{
    /// <summary>
    /// Download users/groups photos info from vk
    /// </summary>
    public class VkPhotosCrawler : ICrawler
    {
        private readonly VkAccessConfig _apiConfig;
        private readonly VkSourseConfig _sourceConfig;
        private readonly VkApi _vkApi;
        private readonly ILogger<VkPhotosCrawler> _logger;

        public VkPhotosCrawler(
            ILogger<VkPhotosCrawler> logger,
            IOptions<VkAccessConfig> apiOptions,
            IOptions<VkSourseConfig> sourceOptions
            )
        {
            _apiConfig = apiOptions.Value;
            _sourceConfig = sourceOptions.Value;
            _logger = logger;
            _vkApi = new VkApi(_logger, _apiConfig);
        }

        public async Task<ContentModel[]> Get(DateTime loadSince)
        {
            var since = loadSince.UnixStamp();
            var tasks = _sourceConfig
                .Albums
                .Select(source => GetPhotos(source, since));
            var results = await Task.WhenAll(tasks);

            return results
                .SelectMany(r => r)
                .ToArray();
        }

        private async Task<ContentModel[]> GetPhotos(AlbumSourceConfig source, long since)
        {
            var albumsIds = source
                .List
                .Select(a => a.AlbumId)
                .ToArray();
            var albumsInfo = await _vkApi.GetAlbums(source.OwnerId, albumsIds);
            var updatedAlbums = albumsInfo.Where(a => a.Updated > since);
            var updatedSources = source
                .List
                .Where(s => updatedAlbums.Any(ua => ua.Id == s.AlbumId));

            var photosTasks = updatedSources.Select(s => LoadAlbumPhotos(source, s, since));
            var photos = await Task.WhenAll(photosTasks);

            return photos
                .SelectMany(p => p)
                .Where(p => !string.IsNullOrEmpty(p.Text))
                .Select(p => new ContentModel()
                {
                    Text = p.Text,
                    Published = DateTimeOffset.FromUnixTimeSeconds(p.DateStamp).DateTime,
                    SourceType = CrawlerType.VkAlbum.ToString(),
                    SourceId = $"{p.OwnerId}",
                    AuthorId = p.AuthorId,
                    Url = p.Url,
                })
                .ToArray();
        }

        //ToDo Load first comment if no photo text provided
        private async Task<PhotoModel[]> LoadAlbumPhotos(
            AlbumSourceConfig source,
            AlbumItemConfig album,
            long sinceStamp
            )
        {
            var result = new List<PhotoModel>();
            var offset = 0;
            var count = 50;
            var requestsCounter = 0;

            while (true)
            {
                var photos = await _vkApi.GetPhotos(source.OwnerId, album.AlbumId, offset, count);
                var validPhotos = photos.Where(p => p.DateStamp > sinceStamp);
                result.AddRange(validPhotos);

                requestsCounter++;

                if (photos.Any(p => p.DateStamp < sinceStamp) ||
                                    result.Count > _sourceConfig.MaxPostsPerGroup ||
                                    result.Count == 0)
                {
                    _logger.LogInformation($"Download {result.Count} photos from [{source.OwnerName}:{album.AlbumName}] ({requestsCounter} request)");
                    return result.ToArray();
                }

                offset += count;
            }
        }
    }
}

