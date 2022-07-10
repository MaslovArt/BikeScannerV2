namespace BikeScanner.Infrastructure.Api
{
    internal interface IApiResult
    {
        bool IsSuccess { get; }
        int? ErrorCode { get; }
        string ErrorMessage { get; }
    }
}

