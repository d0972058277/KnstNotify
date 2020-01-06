using System.Text.Json.Serialization;

namespace KnstNotify.Core.APN
{
    public class ApnResult : ISendResult
    {
        public ApnPayload ApnPayload { get; set; }
        public bool IsSuccess { get; set; }
        public ApnError Error { get; set; }

        public class ApnError
        {
            [JsonPropertyName("reason")]
            public ApnReasonEnum Reason { get; set; }
            [JsonPropertyName("timestamp")]
            public long? Timestamp { get; set; }
        }
    }
}