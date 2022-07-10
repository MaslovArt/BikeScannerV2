using BikeScanner.App.Interfaces;
using BikeScanner.Infrastructure.Crawlers.DirtRu;
using BikeScanner.Infrastructure.Crawlers.Vk;
using BikeScanner.Сonfigs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.ServiceCollection
{
    public static class CrawlersServiceCollection
	{
        public static IServiceCollection AddCrawlers(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddVkCrawlers(configuration);
            services.AddDirtRuCrawler(configuration);

            return services;
        }

        public static IServiceCollection AddVkCrawlers(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.Configure<VkAccessConfig>(configuration.GetSection(nameof(VkAccessConfig)));
            services.Configure<VkSourseConfig>(configuration.GetSection(nameof(VkSourseConfig)));
            services.AddTransient<ICrawler, VkPostsCrawler>();
            services.AddTransient<ICrawler, VkPhotosCrawler>();

            return services;
        }

        public static IServiceCollection AddDirtRuCrawler(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.Configure<DirtRuSourceConfig>(configuration.GetSection(nameof(DirtRuSourceConfig)));
            services.AddTransient<ICrawler, DirtRuCrawler>();

            return services;
        }
    }
}

