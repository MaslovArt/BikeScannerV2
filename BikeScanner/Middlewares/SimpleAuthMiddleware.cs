using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BikeScanner.Core.Extensions;
using BikeScanner.Сonfigs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BikeScanner.Middlewares
{
    /// <summary>
    /// Basic auth for user using login and password from config file
    /// </summary>
    public class SimpleAuthMiddleware
    {
        private readonly RequestDelegate    _next;
        private readonly IConfiguration     _configuration;

        public SimpleAuthMiddleware(
            RequestDelegate next,
            IConfiguration configuration
            )
        {
            _next = next;
            _configuration = configuration;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var authOptions = _configuration
                .GetSectionAs<SimpleAuthConfig>(nameof(SimpleAuthConfig));
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var header = AuthenticationHeaderValue.Parse(authHeader);
                var inBytes = Convert.FromBase64String(header.Parameter);
                var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                if (username.Equals(authOptions.Login) &&
                    password.Equals(authOptions.Password))
                {
                    return _next.Invoke(context);
                }
            }
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return Task.CompletedTask;
        }
    }

}

