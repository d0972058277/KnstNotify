namespace KnstNotify.Core.APN
{
    public class ApnResult : ISendResult
    {
        public bool IsSuccess { get; set; }
        public ApnError Error { get; set; }

        public class ApnError
        {
            public ApnReasonEnum Reason { get; set; }
            public long? Timestamp { get; set; }
        }
    }
}