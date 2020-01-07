using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnstNotify.Core.APN
{
    public interface IApnSender : ISender
    {
        IEnumerable<ApnConfig> Configs { get; }
        Task<ApnResult> SendAsync(ApnPayload notification, ApnOptions options = null);
        Task<ApnResult> SendAsync(ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null);
        Task<ApnResult> SendAsync(ApnPayload notification, ApnConfig apnConfig, ApnOptions options = null);
        Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, ApnOptions options = null);
        Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, Func<IApnSender, ApnConfig> func, ApnOptions options = null);
        Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, ApnConfig apnConfig, ApnOptions options = null);
    }
}