using System.Text;
using Banco.API.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Banco.API.Infrastructure.Messaging
{
    public class RabbitMQConsumer
    {
        private readonly RabbitMQConfig _config;

        public RabbitMQConsumer(IOptions<RabbitMQConfig> config)
        {
            _config = config.Value;
        }

        public void Consume(Action<string> onMessage)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                try
                {
                    onMessage(message);

                    channel.BasicAck(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                }
            };

            channel.BasicConsume(
                queue: _config.QueueName,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}