using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.App.Hangfire
{
    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }
    }
}

