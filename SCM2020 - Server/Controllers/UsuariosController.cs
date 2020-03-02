using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCM2020___Server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using SCM2020___Server.Extensions;
using System.IO;
using Newtonsoft.Json;

namespace SCM2020___Server.Controllers
{
    //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsuariosController : Controller
    {
        UserManager<ApplicationUser> UserManager;
        SignInManager<ApplicationUser> SignInManager;
        IConfiguration Configuration;
        public UsuariosController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Configuration = configuration;
        }
        [HttpGet]
        //[Authorize]
        public ActionResult<string> Get() 
        {
            return "UsuariosController";
        }
        [HttpPost("Criar")]
        [AllowAnonymous]
        //[Authorize(Roles = Roles.SCM)]
        public async Task<ActionResult<UserToken>> CreateUser()
        {
            var postData = await SignUpUserInfo();
            var user = new ApplicationUser { PJERJRegistration = postData.PJERJRegistration };
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, postData.Name),
                new Claim("CPF", postData.CPFRegistration),
                new Claim("PJERJRegistration", postData.PJERJRegistration),
                new Claim(ClaimTypes.Role, postData.Role),
                new Claim("Occupation", postData.Occupation),
            };

            var result = await UserManager.CreateAsync(user, postData.Password);

            //var resultclaims = 
                await UserManager.AddClaimsAsync(
                user: user,
                claims: claims
                );

            if (result.Succeeded)
            {
                return BuildToken(claims);
            }
            else
            {
                return BadRequest($"Usuário ou senha inválidos.\nMatrícula do tribunal: {postData.PJERJRegistration}\nSenha: {postData.Password}");
            }
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login()
        {
            var fromPOST = await SignInUserInfo();
            string strRegistration = fromPOST.Registration;
            
            var user = (fromPOST.IsPJERJRegistration) ? UserManager.FindByPJERJRegistrationAsync(strRegistration) : UserManager.FindByCPFAsync(strRegistration);
            
            var result = await SignInManager.PasswordSignInAsync(
                userName: user.UserName,
                password: fromPOST.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );
            var claims = await UserManager.GetClaimsAsync(user);
           
            if (result.Succeeded)
            {
                return BuildToken(claims.ToArray());
            }
            return BadRequest("Usuário ou senha inválidos.");
        }

        [HttpPost("UpdateData")]
        [Authorize(Roles = Roles.SCM)]
        public async Task<ActionResult<UserToken>> Update()
        {
            var fromPOST = await SignUpUserInfo();
            string strRegistration = (fromPOST.IsPJERJRegistration) ? 
                fromPOST.PJERJRegistration : fromPOST.CPFRegistration;
            
            var user = await UserManager.FindByNameAsync(strRegistration);
            user.PJERJRegistration = fromPOST.PJERJRegistration;
            UserManager.PasswordHasher.HashPassword(user, fromPOST.Password);

            var updateUser = await UserManager.UpdateAsync(user);
            if (updateUser.Succeeded)
            {
                return Ok("Alteração feita com sucesso.");
            }
            return BadRequest();
        }
        public async Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            return Ok("Logoff realizado com sucesso.");
        }
        [HttpPost("ResetPassword")]
        [Authorize(Roles = Roles.SCM)]
        public async Task<ActionResult<UserToken>> ResetPassword()
        {
            var fromPOST = await UpdateUserInfo();
            string strRegistration = fromPOST.Registration;

            string newPassword = fromPOST.NewPassword;

            var user = (fromPOST.IsPJERJRegistration) ? UserManager.FindByPJERJRegistrationAsync(strRegistration) : UserManager.FindByCPFAsync(strRegistration);

            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passwordChangeResult = await UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            var updateUser = await UserManager.UpdateAsync(user);
            var claims = await UserManager.GetClaimsAsync(user);
            if (passwordChangeResult.Succeeded)
            {
                return BuildToken(claims.ToArray());
            }
            return BadRequest();
        }
        [HttpPost("Delete")]
        [Authorize(Roles = Roles.SCM)]
        public IActionResult DeleteUser()
        {
            return Ok();
        }
        private async Task<UpdateUserInfo> UpdateUserInfo()
        {
            var model = JsonConvert.DeserializeObject<UpdateUserInfo>(await Helper.RawFromBody(this));
            return model;
        }
        private UserToken BuildToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Token expiration
            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
        private async Task<SignUpUserInfo> SignUpUserInfo()
        {
            var model = JsonConvert.DeserializeObject<SignUpUserInfo>(await Helper.RawFromBody(this));
            return model;
        }
        private async Task<SignInUserInfo> SignInUserInfo()
        {
            var model = JsonConvert.DeserializeObject<SignInUserInfo>(await Helper.RawFromBody(this));
            return model;

        }
    }
}
