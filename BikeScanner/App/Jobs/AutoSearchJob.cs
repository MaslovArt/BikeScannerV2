using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Jobs.Base;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs
{
    /// <summary>
    /// Search contents by users subs
    /// </summary>
    public class AutoSearchJob : JobBase
    {
        private record AutoSearchSubModel(int Id, long UserId, string SearchQuery);

        private readonly JobExecutionService        _jobExecutionService;
        private readonly SubscriptionsService       _subscriptionsService;
        private readonly ContentService             _contentService;
        private readonly NotificationService        _notificationService;

        public override string JobName => "Subs auto search";

        public AutoSearchJob(
            ILogger<AutoSearchJob> logger,
            JobExecutionService jobExecutionService,
            SubscriptionsService subscriptionsService,
            ContentService contentService,
            NotificationService notificationService
            )
            : base(logger)
        {
            _jobExecutionService = jobExecutionService;
            _subscriptionsService = subscriptionsService;
            _contentService = contentService;
            _notificationService = notificationService;
        }

        public override async Task Run()
        {
            var lastExecuteTime = await _jobExecutionService
                .GetLastAutoSearchTime() ?? new DateTime();

            var subs = await _subscriptionsService.GetAll<AutoSearchSubModel>();
            if (subs.Length == 0)
            {
                LogInformation($"No subscriptions. Skip search");
                return;
            }

            var groupedSubs = subs.GroupBy(s => s.SearchQuery);
            LogInformation($"Total subs: {subs.Length}, uniq subs: {groupedSubs.Count()}");

            var notifications = new List<NotificationQueueModel>();
            foreach (var subGroup in groupedSubs)
            {
                var searchQuery = subGroup.Key;
                var result = await _contentService
                    .Search<ViewContentModel>(searchQuery, 0, 100, lastExecuteTime);
                foreach (var sub in subGroup)
                {
                    var searchNotifications = result.Items
                        .Select(c => new NotificationQueueModel(sub.UserId, $"Новый результат поиска '{searchQuery}'\n\n{c.Url}"));
                    notifications.AddRange(searchNotifications);
                }
            }

            await _notificationService.ScheduleNotifications(notifications);
            await _jobExecutionService.SetLastAutoSearchTime(DateTime.Now);
            LogInformation($"Schedule {notifications.Count} new notifications");
        }
    }
}

