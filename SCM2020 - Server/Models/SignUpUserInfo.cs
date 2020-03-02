using System.ComponentModel.DataAnnotations;

namespace SCM2020___Server.Models
{
    public class SignUpUserInfo : InfoEmployee
    {
        /// <summary>
        /// Checa se é a matrícula do Tribunal de Justiça para o acesso registro.
        /// Caso falso, a ratificação será feita pelo CPF.
        /// </summary>
        [Required(ErrorMessage = "Assinale o tipo de matrícula.")]
        public bool IsPJERJRegistration { get; set; }
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira a senha.")]
        public string Password { get; set; }
    }
}
