using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SCM2020___Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.SCM)]
    public class MovementController : ControllerBase
    {
        UserManager<ApplicationUser> UserManager;
        SignInManager<ApplicationUser> SignInManager;
        IConfiguration Configuration;
        public MovementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Configuration = configuration;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(">> MovementController <<");
        }
        public IActionResult Output()
        {
            return Ok();
        }
        private async Task<string> RawFromBody()
        {
            string postData = string.Empty;
            var stream = Request.Body;
            using (var sr = new StreamReader(stream))
            {
                postData = await sr.ReadToEndAsync();
            }
            return postData;
        }
        private async Task OutputMaterial()
        {
            //var model = JsonConvert.DeserializeObject<Material>(await RawFromBody());
            //return model;

        }
    }
}
