namespace XPike.Logging
{
    public class LogServiceSettings
    {
        public int MaxQueueLength { get; set; }
        
        public int EnqueueTimeoutMs { get; set; }
        
        public LogLevel LogLevel { get; set; }
    }
}