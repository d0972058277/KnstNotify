using System;

namespace KnstNotify.Core.FCM
{
    public class FcmConfig : ISenderConfig
    {
        public string Name { get; set; }
        public string ServerKey { get; }
        public string SenderId { get; }
        public bool? DryRun { get; }

        public FcmConfig(string serverKey, string senderId, bool? dryRun = null)
        {
            ServerKey = serverKey ?? throw new ArgumentNullException(nameof(serverKey));
            SenderId = senderId ?? throw new ArgumentNullException(nameof(senderId));
            DryRun = dryRun;
        }

        public FcmConfig(string serverKey, bool? dryRun = null)
        {
            ServerKey = serverKey ?? throw new ArgumentNullException(nameof(serverKey));
            DryRun = dryRun;
        }

        public string FcmUrl { get; } = "https://fcm.googleapis.com/fcm/send";
    }
}