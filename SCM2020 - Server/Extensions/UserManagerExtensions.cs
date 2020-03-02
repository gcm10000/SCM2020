using Microsoft.AspNetCore.Identity;
using SCM2020___Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Extensions
{
    public static class UserManagerExtensions
    {
        public static ApplicationUser FindByPJERJRegistrationAsync(this UserManager<ApplicationUser> UserManager, string PJERJRegistration)
        {
            return UserManager?.Users?.SingleOrDefault(x => x.PJERJRegistration == PJERJRegistration);
        }
        public static ApplicationUser FindByCPFAsync(this UserManager<ApplicationUser> UserManager, string CPF)
        {
            return UserManager?.Users?.SingleOrDefault(x => x.CPF == CPF);
        }

    }
}
