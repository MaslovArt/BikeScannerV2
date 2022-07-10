using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using BikeScanner.App.Models;
using BikeScanner.DAL;
using BikeScanner.DAL.Extensions;
using BikeScanner.Domain.Models;
using BikeScanner.Domain.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Services
{
    public class NotificationService : AsyncCrudService<NotificationQueue, NotificationQueueModel, NotificationQueueModel>
    {
        private readonly ILogger<NotificationService>   _logger;
        private readonly int                            _notificationСhunkSize;
        private readonly INotificator                   _notificator;

        public NotificationService(
            BikeScannerContext ctx,
            INotificator notificator,
            ILogger<NotificationService> logger
            )
            : base(ctx)
        {
            _notificator = notificator;
            _logger = logger;
            _notificationСhunkSize = 20;
        }

        public Task ScheduleNotification(NotificationQueueModel model) =>
            CreateAsync(model, NotificationQueueStates.Scheduled.ToString());

        public Task ScheduleNotifications(IEnumerable<NotificationQueueModel> models) =>
            CreateManyAsync(models, NotificationQueueStates.Scheduled.ToString());

        public async Task<int> SendAllScheduledNotifications()
        {
            var scheduledNotifications = await repository
                .WithState(NotificationQueueStates.Scheduled)
                .ToArrayAsync();

            var chunks = scheduledNotifications.Chunk(_notificationСhunkSize);
            foreach (var chunk in chunks)
            {
                var notificationTasks = chunk
                    .Select(async notification =>
                    {
                        try
                        {
                            await _notificator.Send(notification.UserId, notification.Text);
                            notification.MarkUpdated();
                            notification.SetState(NotificationQueueStates.Sended);
                        }
                        catch (Exception ex)
                        {
                            notification.MarkUpdated();
                            notification.SetState(NotificationQueueStates.Error);
                            _logger.LogError(ex, $"User [{notification.UserId}] notification err: {ex.Message}");
                        }
                    });

                await Task.WhenAll(notificationTasks);
            }

            ctx.NotificationsQueue.UpdateRange(scheduledNotifications);
            await ctx.SaveChangesAsync();

            return scheduledNotifications.Length;
        }
    }
}

