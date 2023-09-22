namespace EventBusRabbitMQ.Events
{
    public class FileCheckoutEvent
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public byte[] File { get; set; }
    }
}
