using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BikeScanner.App.Models;
using BikeScanner.App.Services;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace BikeScanner.App.Jobs
{
    public class AutoSearchJob
    {
        private record AutoSearchSubModel(int Id, long UserId, string SearchQuery);

        private readonly ILogger<AutoSearchJob>     _logger;
        private readonly JobExecutionService        _jobExecutionService;
        private readonly SubscriptionsService       _subscriptionsService;
        private readonly ContentService             _contentService;
        private readonly NotificationService        _notificationService;

        public AutoSearchJob(
            ILogger<AutoSearchJob> logger,
            JobExecutionService jobExecutionService,
            SubscriptionsService subscriptionsService,
            ContentService contentService,
            NotificationService notificationService
            )
        {
            _logger = logger;
            _jobExecutionService = jobExecutionService;
            _subscriptionsService = subscriptionsService;
            _contentService = contentService;
            _notificationService = notificationService;
        }

        [JobDisplayName("Subs auto search")]
        public async Task Execute()
        {
            var watch = new Stopwatch();
            watch.Start();

            try
            {
                var lastExecuteTime = (await _jobExecutionService
                    .GetLastAutoSearchTime()) ?? new DateTime();

                _logger.LogInformation($"Starting auto search");

                var subs = await _subscriptionsService.GetAll<AutoSearchSubModel>();
                if (subs.Length == 0)
                {
                    _logger.LogInformation($"No subscriptions. Skip search");
                    return;
                }

                var groupedSubs = subs.GroupBy(s => s.SearchQuery);
                _logger.LogInformation($"Total subs: {subs.Length}, uniq subs: {groupedSubs.Count()}");

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
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Auth search error: {ex.Message}");
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation($"Finish auto search in {watch.Elapsed}");
            }
        }
    }
}

