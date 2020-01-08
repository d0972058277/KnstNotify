using System;
using System.Collections.Generic;
using KnstNotify.Core.APN;
using KnstNotify.Core.FCM;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKnstNotify(this IServiceCollection services)
        {
            services.AddHttpClient("APN");
            services.AddHttpClient("FCM");
            services.AddSingleton<IApnSender, ApnSender>();
            services.AddSingleton<IFcmSender, FcmSender>();
            return services;
        }

        public static IServiceCollection AddApnConfig(this IServiceCollection services, ApnConfig config)
        {
            services.AddSingleton<ApnConfig>(config);
            return services;
        }

        public static IServiceCollection AddApnConfig(this IServiceCollection services, Func<ApnConfig> func)
        {
            ApnConfig config = func.Invoke();
            services.AddApnConfig(config);
            return services;
        }

        public static IServiceCollection AddApnConfigs(this IServiceCollection services, IEnumerable<ApnConfig> configs)
        {
            foreach (ApnConfig config in configs)
            {
                services.AddApnConfig(config);
            }
            return services;
        }

        public static IServiceCollection AddApnConfigs(this IServiceCollection services, Func<IEnumerable<ApnConfig>> func)
        {
            IEnumerable<ApnConfig> configs = func.Invoke();
            services.AddApnConfigs(configs);
            return services;
        }

        public static IServiceCollection AddApnConfig(this IServiceCollection services, Func<IServiceProvider, ApnConfig> func)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            ApnConfig config = func(provider);
            services.AddApnConfig(config);
            return services;
        }

        public static IServiceCollection AddApnConfigs(this IServiceCollection services, Func<IServiceProvider, IEnumerable<ApnConfig>> func)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            IEnumerable<ApnConfig> configs = func(provider);
            services.AddApnConfigs(configs);
            return services;
        }

        public static IServiceCollection AddFcmConfig(this IServiceCollection services, FcmConfig config)
        {
            services.AddSingleton<FcmConfig>(config);
            return services;
        }

        public static IServiceCollection AddFcmConfig(this IServiceCollection services, Func<FcmConfig> func)
        {
            FcmConfig config = func.Invoke();
            services.AddFcmConfig(config);
            return services;
        }

        public static IServiceCollection AddFcmConfigs(this IServiceCollection services, IEnumerable<FcmConfig> configs)
        {
            foreach (FcmConfig config in configs)
            {
                services.AddFcmConfig(config);
            }
            return services;
        }

        public static IServiceCollection AddFcmConfigs(this IServiceCollection services, Func<IEnumerable<FcmConfig>> func)
        {
            IEnumerable<FcmConfig> configs = func.Invoke();
            services.AddFcmConfigs(configs);
            return services;
        }

        public static IServiceCollection AddFcmConfig(this IServiceCollection services, Func<IServiceProvider, FcmConfig> func)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            FcmConfig config = func(provider);
            services.AddFcmConfig(config);
            return services;
        }

        public static IServiceCollection AddApnConfigs(this IServiceCollection services, Func<IServiceProvider, IEnumerable<FcmConfig>> func)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            IEnumerable<FcmConfig> configs = func(provider);
            services.AddFcmConfigs(configs);
            return services;
        }
    }
}
