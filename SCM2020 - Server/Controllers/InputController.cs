using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using SCM2020___Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class InputController : ControllerBase
    {
        ControlDbContext context;
        UserManager<ApplicationUser> userManager;
        public InputController(UserManager<ApplicationUser> userManager, ControlDbContext context) { this.userManager = userManager; this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var list = context.MaterialInputByVendor.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.MaterialInputByVendor.Find(id);
            return Ok(list);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Create()
        {
            var raw = await Helper.RawFromBody(this);
            string id = userManager.GetUserId(User);
            var input = new MaterialInputByVendor(raw, id);
            if (!context.ConsumptionProduct.All(x => x.Id == input.Id))
                return BadRequest("Há algum produto na lista não cadastrado. Verifique e tente novamente.");
            context.MaterialInputByVendor.Add(input);
            await context.SaveChangesAsync();
            return Ok("Entrada por fornecedor foi adicionada com sucesso.");
        }
    }
}
