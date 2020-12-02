using Microsoft.AspNetCore.Mvc;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class BusinessController : ControllerBase
    {
        ControlDbContext context;
        public BusinessController(ControlDbContext context)
        {
            this.context = context;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            var business = JsonConvert.DeserializeObject<Business>(raw);

            context.Business.Add(business);
            await context.SaveChangesAsync();
            return Ok("Adicionado com sucesso.");
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            return Ok(context.Business.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var business = context.Business.FirstOrDefault(x => x.Id == id);
            if (business == null)
                return BadRequest($"O registro com o id {id} não existe.");
            return Ok(business);
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var business = JsonConvert.DeserializeObject<Business>(raw);
                business.Id = id;
                context.Business.Update(business);
                await context.SaveChangesAsync();
                return Ok("Atualizado com sucesso.");
            }
        }
        ////Remove by id
        //[HttpDelete("Remove/{id}")]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var obj = context.Business.FirstOrDefault(x => x.Id == id);

        //    //se o usuario está nessa empresa
        //    if (context.MaterialInputByVendor.Any(x => x.VendorId == obj.Id))
        //        return BadRequest("Fornecedor sendo utilizado em alguma entrada.");

        //    context.Business.Remove(obj);
        //    await context.SaveChangesAsync();
        //    return Ok("Removido com sucesso.");
        //}
    }
}
