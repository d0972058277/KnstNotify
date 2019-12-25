using System;

namespace KnstNotify.Core.FCM
{
    public class FcmConfig : ISenderConfig
    {
        public string ServerKey { get; }
        public string SenderId { get; }

        public FcmConfig(string serverKey, string senderId)
        {
            ServerKey = serverKey ?? throw new ArgumentNullException(nameof(serverKey));
            SenderId = senderId ?? throw new ArgumentNullException(nameof(senderId));
        }

        public FcmConfig(string serverKey)
        {
            ServerKey = serverKey ?? throw new ArgumentNullException(nameof(serverKey));
        }

        public string FcmUrl { get; } = "https://fcm.googleapis.com/fcm/send";
    }
}