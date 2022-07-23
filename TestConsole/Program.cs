using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.Infrastructure.Crawlers.DirtRu;
using BikeScanner.ServiceCollection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            Console.ReadKey();
        }


        static ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                }))
                .AddLogging()
                .AddCrawlers(configuration)
                .BuildServiceProvider();

            return serviceProvider;
        }

    }
}

