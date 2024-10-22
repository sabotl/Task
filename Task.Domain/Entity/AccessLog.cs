namespace Task.Domain.Entity
{
    public class AccessLog
    {
        public string IpAddress { get; set; }
        public string Message { get; set; }
        public DateTime AccessTime { get; set; }
    }
}