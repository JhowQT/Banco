namespace Banco.API.Application.DTOs
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }

        public int AgenciaId { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string? CPF { get; set; }

        public string? CNPJ { get; set; }

        public string? RazaoSocial { get; set; }

        public DateTime? DataNascimento { get; set; }
    }
}