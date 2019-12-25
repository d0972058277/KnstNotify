namespace KnstNotify.Core.APN
{
    public class ApnOptions : ISendOptions
    {
        public string ApnsId { get; set; } = null;
        public int ApnsExpiration { get; set; } = 0;
        public int ApnsPriority { get; set; } = 10;
        public bool IsBackground { get; set; } = false;
    }
}