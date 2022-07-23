using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hangfire.Server;
using Hangfire.Console;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs.Base
{
    public abstract class JobBase
    {
        protected readonly ILogger<JobBase> logger;
        protected PerformContext            performContext;

        public JobBase(ILogger<JobBase> logger)
        {
            this.logger = logger;
        }

        public abstract string JobName { get; }
        public abstract Task Run();

        public async Task Execute(PerformContext performContext)
        {
            this.performContext = performContext;

            LogInformation($"{JobName} start");
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                await Run();
                performContext.WriteLine(ConsoleTextColor.Green, "Success");
            }
            catch (Exception ex)
            {
                LogCritical($"{JobName} completed with error: {ex.Message}", ex);
                throw;
            }
            finally
            {
                watch.Stop();
                LogInformation($"{JobName} completed in {watch.Elapsed}");
            }
        }

        protected void LogInformation(string message)
        {
            performContext.WriteLine(message);
            logger.LogInformation(message);
        }

        protected void LogError(string error, Exception ex = null)
        {
            performContext.WriteLine(ConsoleTextColor.Red, error);
            logger.LogError(ex, error);
        }

        protected void LogCritical(string error, Exception ex = null)
        {
            performContext.WriteLine(ConsoleTextColor.Red, error);
            logger.LogCritical(ex, error);
        }
    }
}

