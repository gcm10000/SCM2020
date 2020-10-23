using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
{
    public class SignInUserInfo
    {
        /// <summary>
        /// Matrícula do funcionário cadastrado no Tribunal de Justiça.
        /// </summary>
        [Required]
        public string Register { get; set; }
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
