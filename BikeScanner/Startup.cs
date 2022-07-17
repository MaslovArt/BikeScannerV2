using System;
using BikeScanner.App.Hangfire;
using BikeScanner.Core.Extensions;
using BikeScanner.ServiceCollection;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BikeScanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers().AddNewtonsoftJson();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddPostgresDB(Configuration);
            services.AddAppServices();
            services.AddCrawlers(Configuration);
            services.AddJobs();
            services.AddTelegramNotificator();
            services.AddTelegramBotUI(Configuration);
            services.AddTelegramWebhookHostedService();
            services.AddHangfire(o => o.UsePostgreSqlStorage(Configuration.DefaultConnection()));
            services.AddHangfireServer();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider
            )
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireSimpleAuth(Configuration);
            app.UseHangfireDashboard();

            app.UseSwaggerSimpleAuth(Configuration);
            app.UseSwagger();
            app.UseSwaggerUI();

            JobsSetup.ConfigeJobs(serviceProvider);
        }
    }

}
