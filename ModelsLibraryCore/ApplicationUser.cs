using Microsoft.AspNetCore.Identity;

namespace ModelsLibraryCore
{
    /// <summary>
    /// Modelo de dados utilizado dos funcionários do DETEL pelo Sistema de Login.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Register { get; set; }
        public string Name { get; set; }
        public Sector Sector { get; set; }
        public Business Business { get; set; }
        public int BusinessId { get; set; }

    }
}
