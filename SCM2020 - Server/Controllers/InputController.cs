using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using System.Linq;
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
            var list = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.MaterialInputByVendor.Find(id);
            return Ok(list);
        }
        [HttpGet("Invoice/{invoice}")]
        public IActionResult ShowByInvoice(string invoice)
        {
            var record = context.MaterialInputByVendor.SingleOrDefault(x => x.Invoice == invoice);
            return Ok(record);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            string id = userManager.GetUserId(User);
            var input = new MaterialInputByVendor(raw, id);
            if (context.MaterialInputByVendor.Any(x => x.Invoice == input.Invoice))
                return BadRequest("Já existe uma entrada com esta nota fiscal. Caso queria adicionar um novo produto nesta nota fiscal, atualize a entrada.");

            if (!input.AuxiliarConsumptions.All(x => context.ConsumptionProduct.Any(y => y.Id == x.ProductId)))
                return BadRequest("Há algum produto na lista não cadastrado. Verifique e tente novamente.");
            //Incrementar +1 no produto
            context.MaterialInputByVendor.Add(input);
            await context.SaveChangesAsync();
            return Ok("Entrada por fornecedor foi adicionada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var input = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            input.Id = id;
            context.MaterialInputByVendor.Update(input);
            await context.SaveChangesAsync();
            return Ok("A entrada foi atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {

            var input = context.MaterialInputByVendor.Find(id);
            //var ListAuxiliarConsumption = input.AuxiliarConsumptions;
            context.MaterialInputByVendor.Remove(input);
            await context.SaveChangesAsync();
            return Ok("Entrada foi removida com sucesso.");
        }


    }
}
