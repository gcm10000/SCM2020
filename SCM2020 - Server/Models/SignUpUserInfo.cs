using System.ComponentModel.DataAnnotations;

namespace SCM2020___Server.Models
{
    public class SignUpUserInfo
    {
        /// <summary>
        /// Matrícula do funcionário cadastrado no Tribunal de Justiça.
        /// </summary>
        public string PJERJRegistration { get; set; }
        /// <summary>
        /// Cadastro de Pessoa Física. Recomendado para usuários que ainda não pertecem a matrícula do PJERJ.
        /// Para usuários sem matrícula o CPF é uma opção para efetuar acesso.
        /// </summary>
        public string CPFRegistration { get; set; }
        /// <summary>
        /// Checa se é a matrícula do Tribunal de Justiça para o acesso registro.
        /// Caso falso, a ratificação será feita pelo CPF.
        /// </summary>
        [Required]
        public bool IsPJERJRegistration { get; set; }
        /// <summary>
        /// Nome do funcionário.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Cargo ocupado pelo funcionário.
        /// </summary>
        [Required]
        public string Occupation { get; set; }
        /// <summary>
        /// Setor do funcionário.
        /// </summary>
        [Required]
        public string Role { get; set; }
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
