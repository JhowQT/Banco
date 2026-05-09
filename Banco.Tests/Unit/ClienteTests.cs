using Banco.API.Application.Services;
using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Banco.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Banco.Tests.Unit
{
    public class ClienteTests
    {
        [Fact]
        public async Task Deve_Nao_Permitir_CPF_Duplicado()
        {
            // ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ClienteDbTest")
                .Options;

            using var context = new AppDbContext(options);

            context.Agencias.Add(new Agencia
            {
                Id = 1,
                Nome = "Agencia Central"
            });

            context.PessoasFisicas.Add(new PessoaFisica
            {
                Id = 1,
                CPF = "12345678900",
                DataNascimento = DateTime.Now,
                AgenciaId = 1
            });

            await context.SaveChangesAsync();

            var clienteRepository =
                new ClienteRepository(context);

            var agenciaRepository =
                new AgenciaRepository(context);

            var service = new ClienteService(
                clienteRepository,
                agenciaRepository);

            var novoCliente = new PessoaFisica
            {
                CPF = "12345678900",
                DataNascimento = DateTime.Now,
                AgenciaId = 1
            };

            // ACT + ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.CriarPessoaFisica(novoCliente);
                });

            Assert.Equal(
                "CPF já cadastrado",
                exception.Message);
        }

        [Fact]
        public async Task Deve_Nao_Permitir_Agencia_Inexistente()
        {
            // ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AgenciaDbTest")
                .Options;

            using var context = new AppDbContext(options);

            var clienteRepository =
                new ClienteRepository(context);

            var agenciaRepository =
                new AgenciaRepository(context);

            var service = new ClienteService(
                clienteRepository,
                agenciaRepository);

            var cliente = new PessoaFisica
            {
                CPF = "99999999999",
                DataNascimento = DateTime.Now,
                AgenciaId = 999
            };

            // ACT + ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(
                async () =>
                {
                    await service.CriarPessoaFisica(cliente);
                });

            Assert.Equal(
                "Agência não encontrada",
                exception.Message);
        }
    }
}