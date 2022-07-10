using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BikeScanner.Infrastructure.Api
{
    internal class ApiManager
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly SemaphoreSlim _concurrencySemaphore;

        public ApiManager(ILogger logger, int requestPerSecondLimit)
        {
            _logger = logger;
            _client = new HttpClient();
            _concurrencySemaphore = new SemaphoreSlim(requestPerSecondLimit);
        }

        public async Task<TResult> Request<TResult>(IApiMethod api)
            where TResult : IApiResult
        {
            await _concurrencySemaphore.WaitAsync();

            var requestId = Guid.NewGuid().ToString().Substring(0, 6);
            var requestUrl = api.GetRequestString();

            _logger.LogDebug($"Begin request ({requestId}): {requestUrl}");
            var responseJson = await _client.GetStringAsync(requestUrl);

            //ToDo think about max api request per second.
            if (_concurrencySemaphore.CurrentCount == 0)
                await Task.Delay(1500);
            _concurrencySemaphore.Release();

            _logger.LogDebug($"Response for request {requestId}: {responseJson}");
            var result = JsonSerializer.Deserialize<TResult>(responseJson);
            if (result.IsSuccess)
                _logger.LogDebug($"End request ({requestId}) completed");
            else
                _logger.LogWarning($"End request ({requestId}) failed with code: {result.ErrorCode} and message: {result.ErrorMessage}");

            return result;
        }
    }
}

