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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddPostgresDB(Configuration);
            services.AddAppServices();
            services.AddCrawlers(Configuration);
            services.AddJobs();
            services.AddTelegramNotificator();
            services.AddTelegramBotUI(Configuration);
            services.AddTelegramPollingHostedService();
            services.AddHangfire(o => o.UsePostgreSqlStorage(Configuration.DefaultConnection()));
            services.AddHangfireServer();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider
            )
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            JobsSetup.ConfigeJobs(serviceProvider);
        }
    }
}