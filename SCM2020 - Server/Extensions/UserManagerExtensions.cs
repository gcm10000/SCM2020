using Microsoft.AspNetCore.Identity;
using ModelsLibraryCore;
using System.Linq;

namespace SCM2020___Server.Extensions
{
    public static class UserManagerExtensions
    {
        public static ApplicationUser FindByPJERJRegistrationAsync(this UserManager<ApplicationUser> UserManager, string PJERJRegistration)
        {
            return UserManager.Users.FirstOrDefault(x => x.PJERJRegistration == PJERJRegistration);
        }
        public static ApplicationUser FindByCPFAsync(this UserManager<ApplicationUser> UserManager, string CPF)
        {
            return UserManager.Users.FirstOrDefault(x => x.CPFRegistration == CPF);
        }

    }
}
