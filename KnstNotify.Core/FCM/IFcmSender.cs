using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnstNotify.Core.FCM
{
    public interface IFcmSender
    {
        IEnumerable<FcmConfig> FcmConfigs { get; }
        Task<FcmResult> SendAsync(FcmPayload notification);
        Task<FcmResult> SendAsync(FcmPayload notification, Func<IFcmSender, FcmConfig> func);
        Task<FcmResult> SendAsync(FcmPayload notification, FcmConfig fcmConfig);
        Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications);
        Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications, Func<IFcmSender, FcmConfig> func);
        Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications, FcmConfig fcmConfig);
    }
}