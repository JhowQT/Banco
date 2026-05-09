using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.DTOs
{
    public class AgenciaUpdateDTO
    {
        [Required(ErrorMessage = "O nome da agência é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
    }
}