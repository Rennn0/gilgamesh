namespace Consumer.Settings
{
    public class RabbitMqSettings
    {
        public int Port { get; set; }
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
    }
}