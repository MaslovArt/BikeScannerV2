using System;
using BikeScanner.Infrastructure.Api;
using BikeScanner.Infrastructure.Crawlers.Vk.Models;
using BikeScanner.Сonfigs;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Api.Methods
{
    /// <summary>
    /// Base class for vk api methods
    /// </summary>
    internal abstract class VkApiMethod : IApiMethod
    {
        private readonly string _apiVersion,
                                _accessToken;

        public VkApiMethod(VkAccessConfig settings)
        {
            _accessToken = settings.ApiKey
                ?? throw new ArgumentNullException(nameof(settings.ApiKey));
            _apiVersion = settings.Version
                ?? throw new ArgumentNullException(nameof(settings.Version));
        }

        /// <summary>
        /// Method name
        /// </summary>
        public abstract string Method { get; }

        /// <summary>
        /// Method url format params
        /// </summary>
        public abstract string Params { get; }

        public string GetRequestString() =>
            $"https://api.vk.com/method/{Method}?access_token={_accessToken}&{Params}&v={_apiVersion}";
    }
}

