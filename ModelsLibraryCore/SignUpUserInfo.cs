using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
{
    public class SignUpUserInfo : InfoEmployee
    {
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira a senha.")]
        public string Password { get; set; }
    }
}
