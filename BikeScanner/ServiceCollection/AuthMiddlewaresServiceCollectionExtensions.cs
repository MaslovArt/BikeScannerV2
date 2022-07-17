using System;
using BikeScanner.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BikeScanner.ServiceCollection
{
	public static class AuthMiddlewaresServiceCollectionExtensions
	{
		public static IApplicationBuilder UseSwaggerSimpleAuth(
			this IApplicationBuilder app,
			IConfiguration configuration
			) =>
			app.UseMiddleware<SimpleAuthMiddleware>("/swagger");

        public static IApplicationBuilder UseHangfireSimpleAuth(
			this IApplicationBuilder app,
			IConfiguration configuration
			) =>
			app.UseMiddleware<SimpleAuthMiddleware>("/hangfire");
    }
}

