using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;

namespace SCM2020___Server.Models
{
    /// <summary>
    /// Modelo de dados utilizado dos funcionários do Tribunal de Justiça DGSEI - DETEL pelo Sistema de Login.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        
        /// <summary>
        /// Matrícula do usuário pelo Tribunal de Justiça.
        /// </summary>
        public string PJERJRegistration { get; set; }
        /// <summary>
        /// CPF do funcionário.
        /// </summary>
        public string CPF { get; set; }
    }
}
