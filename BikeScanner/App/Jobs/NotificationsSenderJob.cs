using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BikeScanner.App.Services;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs
{
    public class NotificationsSenderJob
    {
        private readonly NotificationService _notificationService;
        private readonly ILogger<NotificationsSenderJob> _logger;

        public NotificationsSenderJob(
            ILogger<NotificationsSenderJob> logger,
            NotificationService notificationService
            )
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [JobDisplayName("Send notifications")]
        public async Task Execute()
        {
            var notifyWatch = new Stopwatch();
            notifyWatch.Start();

            _logger.LogInformation("Start notifying");
            try
            {
                var total = await _notificationService.SendAllScheduledNotifications();
                _logger.LogInformation($"Process {total} notifications");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Notifying error: {ex.Message}");
                throw;
            }
            finally
            {
                notifyWatch.Stop();
                _logger.LogInformation($"Finish notifying in {notifyWatch.Elapsed}");
            }
        }
    }
}

