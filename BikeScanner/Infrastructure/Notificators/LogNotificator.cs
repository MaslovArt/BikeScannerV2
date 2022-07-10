using System.Threading.Tasks;
using BikeScanner.App.Interfaces;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Notificators
{
    public class LogNotificator : INotificator
    {
        private readonly ILogger<LogNotificator> _logger;

        public LogNotificator(ILogger<LogNotificator> logger)
        {
            _logger = logger;
        }

        public Task Send(long userId, string message)
        {
            _logger.LogInformation($"For [{userId}] => {message}");
            return Task.CompletedTask;
        }
    }
}

