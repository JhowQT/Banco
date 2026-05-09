namespace Banco.API.Configurations
{
    public class RabbitMQConfig
    {
        public string HostName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;
    }
}