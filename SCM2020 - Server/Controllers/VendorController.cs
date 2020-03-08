using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using ModelsLibrary;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class VendorController : ControllerBase
    {
        ControlDbContext context;
        public VendorController(ControlDbContext context) { this.context = context; }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            var vendor = JsonConvert.DeserializeObject<Vendor>(raw);

            context.Vendors.Add(vendor);
            await context.SaveChangesAsync();
            return Ok("Adicionado com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var vendor = JsonConvert.DeserializeObject<Vendor>(raw);
                vendor.Id = id;
                context.Vendors.Update(vendor);
                await context.SaveChangesAsync();
                return Ok("Atualizado com sucesso.");
            }
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            return Ok(context.Vendors.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == id);
            if (vendor == null)
                return BadRequest($"O registro com o id {id} não existe.");
            return Ok(vendor);
        }
        //Remove by id
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var obj = context.Vendors.FirstOrDefault(x => x.Id == id);

            if (context.MaterialInputByVendor.Any(x => x.VendorId == obj.Id))
                return BadRequest("Fornecedor sendo utilizado em alguma entrada.");

            context.Vendors.Remove(obj);
            await context.SaveChangesAsync();
            return Ok("Removido com sucesso.");
        }
    }
}
