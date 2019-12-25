using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnstNotify.Core.APN
{
    public interface IApnSender : ISender
    {
        IEnumerable<ApnConfig> ApnConfigs { get; }
        Task<ApnResult> SendAsync(string deviceToken, ApnPayload notification, ApnOptions options = null);
        Task<ApnResult> SendAsync(string deviceToken, ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null);
        IEnumerable<ApnResult> Send(IEnumerable<string> deviceTokens, ApnPayload notification, ApnOptions options = null);
        IEnumerable<ApnResult> Send(IEnumerable<string> deviceTokens, ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null);
    }
}