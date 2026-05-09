using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.DTOs
{
    public class ContratacaoCreateDTO
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProdutoId { get; set; }
    }
}