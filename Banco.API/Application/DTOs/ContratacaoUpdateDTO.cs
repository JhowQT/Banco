using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.DTOs
{
    public class ContratacaoUpdateDTO
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}