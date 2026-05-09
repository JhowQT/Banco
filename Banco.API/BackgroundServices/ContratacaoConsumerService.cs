using System.Text.Json;
using Banco.API.Infrastructure.Messaging;
using Banco.API.Infrastructure.Repositories;

namespace Banco.API.BackgroundServices
{
    public class ContratacaoConsumerService : BackgroundService
    {
        private readonly RabbitMQConsumer _consumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public ContratacaoConsumerService(
            RabbitMQConsumer consumer,
            IServiceScopeFactory scopeFactory)
        {
            _consumer = consumer;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Consume(async (message) =>
            {
                Console.WriteLine($"Mensagem recebida: {message}");

                using var scope = _scopeFactory.CreateScope();

                var repository =
                    scope.ServiceProvider.GetRequiredService<ContratacaoRepository>();

                var data = JsonSerializer.Deserialize<ContratacaoMessage>(message);

                if (data == null)
                    throw new Exception("Mensagem inválida");

                var contratacao =
                    await repository.GetByIdAsync(data.Id);

                if (contratacao == null)
                    throw new Exception("Contratação não encontrada");

                // SIMULA PROCESSAMENTO
                await Task.Delay(3000);

                contratacao.Status = "APROVADA";

                await repository.UpdateAsync(contratacao);

                Console.WriteLine(
                    $"Contratação {contratacao.Id} aprovada");
            });

            return Task.CompletedTask;
        }

        private class ContratacaoMessage
        {
            public int Id { get; set; }

            public int ClienteId { get; set; }

            public int ProdutoId { get; set; }
        }
    }
}