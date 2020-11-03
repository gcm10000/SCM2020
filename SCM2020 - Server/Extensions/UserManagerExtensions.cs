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
        public static ApplicationUser FindByRegister(this UserManager<ApplicationUser> UserManager, string Register)
        {
            return UserManager.Users.FirstOrDefault(x => x.Register == Register);
        }
        public static ApplicationUser FindUserById(this UserManager<ApplicationUser> UserManager, string Id)
        {
            return UserManager.Users.FirstOrDefault(x => x.Id == Id);
        }
        public static ApplicationUser FindByFullName(this UserManager<ApplicationUser> UserManager, string Name)
        {
            return UserManager.Users.FirstOrDefault(x => x.Name == Name);
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
