using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.DTOs
{
    public class ClientePJUpdateDTO
    {
        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [MaxLength(18)]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "Razão Social é obrigatória")]
        [MaxLength(150)]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required]
        public int AgenciaId { get; set; }
    }
}