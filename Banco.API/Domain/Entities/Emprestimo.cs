namespace Banco.API.Domain.Entities
{
    public class Emprestimo : Produto
    {
        public decimal Valor { get; set; }

        public decimal TaxaJuros { get; set; }
    }
}