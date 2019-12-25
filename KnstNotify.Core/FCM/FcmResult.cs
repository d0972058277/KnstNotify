using System.Collections.Generic;

namespace KnstNotify.Core.FCM
{
    public class FcmResult : ISendResult
    {
        public decimal multicast_id { get; set; }
        public decimal success { get; set; }
        public decimal failure { get; set; }
        public IEnumerable<FcmResultInfo> results { get; set; }

        public class FcmResultInfo
        {
            public string message_id { get; set; }
            public string registration_id { get; set; }
            public FcmReasonEnum? error { get; set; }
        }
    }
}