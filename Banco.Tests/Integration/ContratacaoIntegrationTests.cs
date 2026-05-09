using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace Banco.Tests.Integration
{
    public class ContratacaoIntegrationTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContratacaoIntegrationTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Deve_Acessar_Swagger_Com_Sucesso()
        {
            // ACT
            var response =
                await _client.GetAsync("/swagger/index.html");

            // ASSERT
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
    }
}