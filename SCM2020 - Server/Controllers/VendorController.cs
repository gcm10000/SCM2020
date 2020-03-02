using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using SCM2020___Server.Models;
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
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var vendor = JsonConvert.DeserializeObject<Vendor>(raw);

                context.Vendors.Add(vendor);
                await context.SaveChangesAsync();
                return Ok("Adicionado com sucesso.");
            }
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
            using (context)
            {
                var tojson = JsonConvert.SerializeObject(context.Vendors.ToArray());
                return Ok(tojson);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            using (context)
            {
                var vendor = context.Vendors.FirstOrDefault(x => x.Id == id);
                if (vendor != null)
                {
                    var tojson = JsonConvert.SerializeObject(vendor);
                    return Ok(tojson);
                }
                else
                {
                    return BadRequest($"O registro com o id {id} não existe.");
                }
            }
        }
        //Remove by id
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var id = JsonConvert.DeserializeObject<int>(raw);
                var obj = context.Vendors.FirstOrDefault(x => x.Id == id);
                context.Vendors.Remove(obj);
                await context.SaveChangesAsync();
                return Ok("Removido com sucesso.");
            }
        }
        //
    }
}
