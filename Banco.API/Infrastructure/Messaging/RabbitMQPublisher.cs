using System.Text;
using System.Text.Json;
using Banco.API.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Banco.API.Infrastructure.Messaging
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQConfig _config;

        public RabbitMQPublisher(IOptions<RabbitMQConfig> config)
        {
            _config = config.Value;
        }

        public void Publish(object message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: _config.QueueName,
                basicProperties: properties,
                body: body
            );
        }
    }
}