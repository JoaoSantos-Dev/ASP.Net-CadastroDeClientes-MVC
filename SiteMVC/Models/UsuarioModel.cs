using SiteMVC.Enums;
using System.ComponentModel.DataAnnotations;

namespace SiteMVC.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do contato")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o login do contato")]
        [EmailAddress(ErrorMessage = "O email informado não é válido")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite o email do contato")]
        [EmailAddress(ErrorMessage = "O email informado não é válido")]
        public string Email { get; set; }
        public byte[]? Foto { get; set; }
        public PerfilEnum Perfil { get; set; }
        [Required(ErrorMessage = "Digite a senha do contato")]
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
