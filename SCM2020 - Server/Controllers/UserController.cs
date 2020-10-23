﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelsLibraryCore;
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
using SCM2020___Server.Context;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using SCM2020___Server.Hubs;
using Microsoft.EntityFrameworkCore;

namespace SCM2020___Server.Controllers
{
    //Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : Controller
    {
        ApplicationDbContext ApplicationDbContext;
        ControlDbContext ControlDbContext;
        UserManager<ApplicationUser> UserManager;
        SignInManager<ApplicationUser> SignInManager;
        IConfiguration Configuration;
        IHubContext<NotifyHub> Notification;
        static bool EventActived = false;
        static List<StoreMessage> ListStoreMessage = new List<StoreMessage>();

        public UserController(ApplicationDbContext applicationDbContext, ControlDbContext controlDbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IHubContext<NotifyHub> Notification)
        {
            this.ApplicationDbContext = applicationDbContext;
            this.ControlDbContext = controlDbContext;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Configuration = configuration;
            this.Notification = Notification;

            Helper.Users = new List<ApplicationUser>(userManager.Users);
            if (!EventActived)
            {
                EventActived = true;
                ConsumptionProduct.ValueChanged += ConsumptionProduct_ValueChanged;
            }

            //Task.Run(SaveData);
        }

        private async void SaveData()
        {
            if (ListStoreMessage.Count == 0)
                return;
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<ControlDbContext>();
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            using (var context = new Context.ControlDbContext(options.Options))
            {
                context.StoreMessage.AddRange(ListStoreMessage);
                await context.SaveChangesAsync();
                ListStoreMessage.Clear();
            }
        }

        private void ConsumptionProduct_ValueChanged(ConsumptionProduct ConsumptionProduct, EventArgs e)
        {
            //var users = Helper.Users;
            var SCM = Helper.Users.Where(x => x.SectorId == 2);
            List<Destination> destination = new List<Destination>();
            foreach (var user in SCM)
            {
                destination.Add(new Destination() { Id = 0, UserId = user.Id });
            }

            if (ConsumptionProduct.Stock < ConsumptionProduct.MininumStock)
            {
                //SendMessage($"Produto {ConsumptionProduct.Code} - {ConsumptionProduct.Description} está com estoque deficiente.");
                SendMessage(new AlertStockMessage(ToolTipIcon.Error, $"Produto {ConsumptionProduct.Code} - {ConsumptionProduct.Description} está com estoque deficiente.", destination, ConsumptionProduct.Code, ConsumptionProduct.Description));
            }

            if (ConsumptionProduct.Stock > ConsumptionProduct.MaximumStock)
            {
                //Envia aos clientes com a role SCM alertando material com muito estoque
                SendMessage(new AlertStockMessage(ToolTipIcon.Error, $"Produto {ConsumptionProduct.Code} - {ConsumptionProduct.Description} está com estoque excedente.", destination, ConsumptionProduct.Code, ConsumptionProduct.Description));
            }
        }
        private async void SendMessage(INotification notification)
        {
            var onlineSCM = NotifyHub.Connections.GetAllUser();
            List<string> usersIdDisconnected = new List<string>();
            foreach (var userSCM in notification.Destination)
            {
                var user = onlineSCM.SingleOrDefault(x => x.Id == userSCM.UserId);
                //Dentro deste if a mensagem é enviada ao cliente
                if (user != null)
                {
                    var uniqueID = NotifyHub.Connections.GetKey(user);
                    Console.WriteLine(notification);
                    await Notification.Clients.Client(uniqueID).SendAsync("notify", notification.ToJson());
                }
                else //Dentro deste else indica que o usuário está desconectado
                {
                    usersIdDisconnected.Add(userSCM.UserId);
                }
            }

            if (usersIdDisconnected.Count > 0)
            {
                ListStoreMessage.Add(StoreMessage(notification, usersIdDisconnected));
                SaveData();
            }
            
        }
        private StoreMessage StoreMessage(INotification notification, List<string> UsersId)
        {
            List<UsersId> usersIds = new List<UsersId>();
            foreach (var userid in UsersId)
            {
                usersIds.Add(new ModelsLibraryCore.UsersId(userid));
            }
            StoreMessage sMessage = new StoreMessage(notification, usersIds);
            return sMessage;
        }

        [HttpGet("Get")]
        [Authorize(Roles = Roles.Administrator)]
        public ActionResult<string> Get() 
        {
            return "UsuariosController";
        }
        [HttpPost("NewUser")]
        //[Authorize(Roles = Roles.Administrator + "," + Roles.SCM)]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> CreateUser()
        {
            var postData = await SignUpUserInfo();
            var username = postData.Register;

            var user = new ApplicationUser { UserName = username, Name = postData.Name, Register = username, SectorId = postData.Sector };
            
            var r = UserManager.FindByRegister(postData.Register);

            if (r != null)
                return BadRequest("Já existe um usuário com este registro.");

            var result = await UserManager.CreateAsync(user, postData.Password);

            if (result.Succeeded)
            {

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserManager.Users.SingleOrDefault(x => x == user).Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.SectorId.ToString()),
                };

                return BuildToken(claims);
            }
            else
            {
                string strerror = "Não foi possível efetuar o cadastro.\n";
                foreach (var error in result.Errors.ToArray())
                {
                    strerror += $"{error.Code}: {error.Description}\n";
                }
                return BadRequest(strerror);
            }
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login()
        {
            var fromPOST = await SignInUserInfo();
            string strRegistration = fromPOST.Register;

            var user = UserManager.FindByRegister(strRegistration);

            if (user == null)
                return BadRequest("Usuário ou senha inválidos.");

            var result = await SignInManager.PasswordSignInAsync(
                userName: user.UserName,
                password: fromPOST.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );
            //var claims = await UserManager.GetClaimsAsync(user);

            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, UserManager.Users.SingleOrDefault(x => x == user).Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.SectorId.ToString()),
            };

            if (result.Succeeded)
            {
                return BuildToken(claims);
            }
            return BadRequest("Usuário ou senha inválidos.");
        }

        [HttpPost("UpdateData")]
        [Authorize(Roles = Roles.SCM)]
        public async Task<ActionResult<UserToken>> Update()
        {
            var fromPOST = await SignUpUserInfo();

            var user = await UserManager.FindByNameAsync(fromPOST.Register);
            UserManager.PasswordHasher.HashPassword(user, fromPOST.Password);

            var updateUser = await UserManager.UpdateAsync(user);
            if (updateUser.Succeeded)
            {
                return Ok("Alteração feita com sucesso.");
            }
            return BadRequest();
        }
        //[HttpGet("SignOut")]
        //public async Task<IActionResult> SignOut()
        //{
        //    await SignInManager.SignOutAsync();
        //    return Ok("Logoff realizado com sucesso.");
        //}
        [HttpPost("ResetPassword")]
        [Authorize(Roles = Roles.SCM)]
        public async Task<ActionResult<UserToken>> ResetPassword()
        {
            var fromPOST = await UpdateUserInfo();
            string strRegistration = fromPOST.Register;

            string newPassword = fromPOST.NewPassword;

            var user = UserManager.FindByRegister(strRegistration);

            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passwordChangeResult = await UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            var updateUser = await UserManager.UpdateAsync(user);

            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, UserManager.Users.SingleOrDefault(x => x == user).Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.SectorId.ToString()),
            }
            ;
            if (passwordChangeResult.Succeeded)
            {
                return BuildToken(claims);
            }
            return BadRequest();
        }
        [HttpPost("ExistsName")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ExistsName()
        {
            var raw = await Helper.RawFromBody(this);
            var name = JsonConvert.DeserializeObject<string>(raw);

            var result = UserManager.Users.Any(x => x.Name == name);
            return result;
        }
        [HttpGet("UserId/{register}")]
        [AllowAnonymous]
        public ActionResult<string> GetUserIdByPJERJRegister(string register)
        {
            var user = UserManager.FindByRegister(register);
            if (user != null)
            {
                return user.Id;
            }
            return BadRequest("Matrícula não encontrada.");
        }
        [HttpGet("RegisterId/{userId}")]
        [AllowAnonymous]
        public ActionResult<string> GetPJERJRegisterByUserId(string userId)
        {
            var user = UserManager.FindUserById(userId);
            if (user != null)
            {
                return user.Register;
            }
            return BadRequest("Id não encontrado.");
        }
        [HttpGet("InfoUser/{userId}")]
        [AllowAnonymous]
        public ActionResult<InfoUser> GetInfoUserById(string userId)
        {
            var user = UserManager.FindUserById(userId);
            if (user != null)
            {
                //var currentSector = ControlDbContext.Sectors.First(x => x.Id == user.IdSector);
                var currentSector = ControlDbContext.Sectors.First(x => x.Id == 1);
                return new InfoUser(user.Id, user.Name, user.Register, string.Empty, currentSector);
            }
            return BadRequest("Id não encontrado.");
        }
        [HttpGet("InfoUserRegister/{register}")]
        [AllowAnonymous]
        public ActionResult<InfoUser> GetInfoUserByRegister(string register)
        {
            if (UserManager.Users.Any(x => x.UserName == register))
            {
                var user = UserManager.Users.First(x => x.UserName == register);
                var currentSector = ControlDbContext.Sectors.First(x => x.Id == 1);
                return new InfoUser(user.Id, user.Name, user.Register, string.Empty, currentSector);
            }
            return BadRequest("Não existe um funcionário com esta matrícula.");
        }
        [HttpGet("search/{query}")]
        [AllowAnonymous]
        public ActionResult<InfoUser> SearchByQuery(string query)
        {
            query = System.Uri.UnescapeDataString(query);

            if (string.IsNullOrWhiteSpace(query))
                return Ok();

            string[] querySplited = query.Trim().Split(' ');

            var allUsers = UserManager.Users.ToList();

            var AppUsers = allUsers.Where(x => x.Name.MultiplesContainsWords(querySplited) || x.Register.Contains(query));
            System.Collections.Generic.List<InfoUser> infoUsers = new System.Collections.Generic.List<InfoUser>();
            foreach (var AppUser in AppUsers)
            {
                infoUsers.Add(new InfoUser(AppUser.Id, AppUser.Name, AppUser.Register, string.Empty, ControlDbContext.Sectors.Find(AppUser.SectorId)));
            }
            return Ok(infoUsers);

        }
        
        [HttpGet("search")]
        [AllowAnonymous]
        public ActionResult<InfoUser> ListAll()
        {
            var allUsers = UserManager.Users.ToList();

            System.Collections.Generic.List<InfoUser> infoUsers = new System.Collections.Generic.List<InfoUser>();
            foreach (var AppUser in allUsers)
            {
                infoUsers.Add(new InfoUser(AppUser.Id, AppUser.Name, AppUser.Register, string.Empty, ControlDbContext.Sectors.Find(AppUser.SectorId)));
            }
            return Ok(infoUsers);

        }
        [HttpGet("ListUser/{query}")]
        [AllowAnonymous]
        public IActionResult GetListUser(string query)
        {
            System.Collections.Generic.List<ApplicationUser> listUser = (query != string.Empty) ? UserManager.Users.Where(x => 
            x.Register.MultiplesContains(query) || x.Name.MultiplesContains(query)).ToList() 
            : UserManager.Users.ToList();
            return Ok(listUser);
        }
        //[HttpDelete("Delete")]
        //[Authorize(Roles = Roles.SCM)]
        //public IActionResult DeleteUser()
        //{
        //    return Ok();
        //}
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
            var expiration = DateTime.UtcNow.AddMinutes(60);
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
