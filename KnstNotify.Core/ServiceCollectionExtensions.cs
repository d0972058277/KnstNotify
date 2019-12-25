using System;
using System.Collections.Generic;
using KnstNotify.Core.APN;
using KnstNotify.Core.FCM;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKnsNotify(this IServiceCollection services)
        {
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
            services.AddSingleton<ApnConfig>(config);
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

        public static IServiceCollection AddFcmConfig(this IServiceCollection services, FcmConfig config)
        {
            services.AddSingleton<FcmConfig>(config);
            return services;
        }

        public static IServiceCollection AddFcmConfig(this IServiceCollection services, Func<FcmConfig> func)
        {
            FcmConfig config = func.Invoke();
            services.AddSingleton<FcmConfig>(config);
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
    }
}
