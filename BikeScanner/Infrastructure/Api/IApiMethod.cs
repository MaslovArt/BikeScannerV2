namespace BikeScanner.Infrastructure.Api
{
    /// <summary>
    /// Api request
    /// </summary>
    internal interface IApiMethod
    {
        /// <summary>
        /// Get api request url
        /// </summary>
        /// <returns></returns>
        string GetRequestString();
    }
}

