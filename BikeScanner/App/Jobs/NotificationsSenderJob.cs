using System.Threading.Tasks;
using BikeScanner.App.Jobs.Base;
using BikeScanner.App.Services;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Send scheduled notifications
    /// </summary>
    public class NotificationsSenderJob : JobBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsSenderJob(
            ILogger<NotificationsSenderJob> logger,
            NotificationService notificationService
            )
            : base(logger)
        {
            _notificationService = notificationService;
        }

        public override string JobName => "Notificator";

        public override async Task Run()
        {
            var total = await _notificationService.SendAllScheduledNotifications();
            LogInformation($"Process {total} notifications");
        }

    }
}

