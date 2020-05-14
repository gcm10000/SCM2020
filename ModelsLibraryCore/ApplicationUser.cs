using Microsoft.AspNetCore.Identity;

namespace ModelsLibraryCore
{
    /// <summary>
    /// Modelo de dados utilizado dos funcionários do Tribunal de Justiça DGSEI - DETEL pelo Sistema de Login.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string PJERJRegistration { get; set; }
        public string CPFRegistration { get; set; }
        public string Name { get; set; }
    }
}
