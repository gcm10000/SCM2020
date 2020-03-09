
namespace ModelsLibrary
{
    public class SignInUserInfo
    {
        /// <summary>
        /// Matrícula do funcionário cadastrado no Tribunal de Justiça.
        /// </summary>
        public string Registration { get; set; }
        /// <summary>
        /// Checa se é a matrícula do Tribunal de Justiça para o acesso registro.
        /// Caso falso, a ratificação será feita pelo CPF.
        /// </summary>
        public bool IsPJERJRegistration { get; set; }
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        public string Password { get; set; }
    }
}
