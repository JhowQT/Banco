namespace Banco.API.Domain.Entities
{
    public class Agencia
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public ICollection<Cliente> Clientes { get; set; }
            = new List<Cliente>();
    }
}