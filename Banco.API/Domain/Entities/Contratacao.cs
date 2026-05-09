namespace Banco.API.Domain.Entities
{
    public class Contratacao
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public int ProdutoId { get; set; }

        public Produto? Produto { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }
    }
}