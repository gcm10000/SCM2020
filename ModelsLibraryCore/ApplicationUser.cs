using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    /// <summary>
    /// Modelo de dados utilizado dos funcionários do DETEL pelo Sistema de Login.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Register { get; set; }
        public string Name { get; set; }
        public int? SectorId { get; set; }
        public int? BusinessId { get; set; }

    }
}
