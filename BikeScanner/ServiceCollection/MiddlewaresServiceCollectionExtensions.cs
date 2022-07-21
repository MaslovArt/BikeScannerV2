using BikeScanner.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BikeScanner.ServiceCollection
{
    public static class MiddlewaresServiceCollectionExtensions
	{
		public static IApplicationBuilder UseSimpleAuth(this IApplicationBuilder app) =>
			app.UseMiddleware<SimpleAuthMiddleware>();
    }
}

