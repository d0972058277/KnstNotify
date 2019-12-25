using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KnstNotify.Core.FCM
{
    public class FcmResult : ISendResult
    {
        [JsonPropertyName("multicast_id")]
        public decimal MulticastId { get; set; }
        [JsonPropertyName("success")]
        public decimal Success { get; set; }
        [JsonPropertyName("failure")]
        public decimal Failure { get; set; }
        [JsonPropertyName("results")]
        public IEnumerable<FcmResultInfo> Results { get; set; }

        public class FcmResultInfo
        {
            [JsonPropertyName("message_id")]
            public string MessageId { get; set; }
            [JsonPropertyName("registration_id")]
            public string RegistrationId { get; set; }
            [JsonPropertyName("error")]
            public FcmReasonEnum? Error { get; set; }
        }
    }
}