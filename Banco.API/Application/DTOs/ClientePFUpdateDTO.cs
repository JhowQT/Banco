using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.DTOs
{
    public class ClientePFUpdateDTO
    {
        [Required(ErrorMessage = "CPF é obrigatório")]
        [MaxLength(14)]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public int AgenciaId { get; set; }
    }
}