using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ModelsLibraryCore;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SCM2020___Server.Extensions
{
    public static class UserManagerExtensions
    {
        public static ApplicationUser FindByPJERJRegistration(this UserManager<ApplicationUser> UserManager, string PJERJRegistration)
        {
            return UserManager.Users.FirstOrDefault(x => x.PJERJRegistration == PJERJRegistration);
        }
        public static ApplicationUser FindUserById(this UserManager<ApplicationUser> UserManager, string Id)
        {
            return UserManager.Users.FirstOrDefault(x => x.Id == Id);
        }
        public static ApplicationUser FindByCompleteName(this UserManager<ApplicationUser> UserManager, string Name)
        {
            return UserManager.Users.FirstOrDefault(x => x.Name == Name);
        }
        public static ApplicationUser FindByCPF(this UserManager<ApplicationUser> UserManager, string CPF)
        {
            return UserManager.Users.FirstOrDefault(x => x.CPFRegistration == CPF);
        }
        public static async Task<ApplicationUser> GetUserByNameClaim(this UserManager<ApplicationUser> UserManager, string name)
        {
            foreach (var user in UserManager.Users)
            {
                var claims = await UserManager.GetClaimsAsync(user);
                if (claims.Contains(new Claim(ClaimTypes.Name, name)))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
