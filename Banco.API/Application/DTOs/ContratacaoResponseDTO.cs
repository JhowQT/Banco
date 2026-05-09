namespace Banco.API.Application.DTOs
{
    public class ContratacaoResponseDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int ProdutoId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }
    }
}