using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class DevolutionController : ControllerBase
    {
        ControlDbContext context;
        public DevolutionController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).ToList();
            return Ok(devolution);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).SingleOrDefault(x => x.Id == id);
            return Ok(devolution);
        }
        [HttpGet("WorkOrder/{workorder}")]
        public IActionResult ShowByWorkOrder(string workorder)
        {
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).SingleOrDefault(x => x.WorkOrder == workorder);
            return Ok(devolution);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            bool b = Helper.GetToken(out System.IdentityModel.Tokens.Jwt.JwtSecurityToken token, this);
            if (!b)
                return BadRequest("Por favor, faça login.");
            var id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            MaterialInput materialInput = new MaterialInput(raw, id);
            var output = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts)
                .FirstOrDefault(x => x.WorkOrder == materialInput.WorkOrder);
            if (output == null)
                return BadRequest("Não há qualquer saída existente nessa ordem de serviço.");
            bool allMatches = materialInput.ConsumptionProducts
                .All(x => output.ConsumptionProducts
                .Any(y => y.ProductId == x.ProductId));
            if (!allMatches)
                return BadRequest("Existem itens que estão na entrada e que não pertencem a saída.");
            var allMatches2 = materialInput.ConsumptionProducts
                .Where(x => output.ConsumptionProducts
                .Any(y => x.Quantity > y.Quantity));
            if (allMatches2.Count() > 0)
            {
                string names = string.Empty;
                foreach (var match in allMatches2)
                {
                    names += context.ConsumptionProduct.Find(match.ProductId).Description + "\n";
                }
                return BadRequest($"O seguintes materiais estão com valor acima da quantidade que foi solicitada na saída:\n{names}");

            }

            context.MaterialInput.Add(materialInput);
            //Adicionar produtos
            foreach (var item in materialInput.ConsumptionProducts)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock += item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            foreach (var p in materialInput.PermanentProducts)
            {
                var c = context.ConsumptionProduct.Find(p.ProductId);
                c.Stock += 1;
                context.ConsumptionProduct.Update(c);
            }

            await context.SaveChangesAsync();
            return Ok("Nova devolução registrada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var materialInput = JsonConvert.DeserializeObject<MaterialInput>(raw);
            materialInput.Id = id;
            if (context.Monitoring.Any(x => (x.Work_Order == materialInput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");

            context.MaterialInput.Update(materialInput);
            return Ok("Devolução atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var materialInput = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);
            if (context.Monitoring.Any(x => (x.Work_Order == materialInput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");
            foreach (var item in materialInput.ConsumptionProducts)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock -= item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            foreach (var c in materialInput.PermanentProducts)
            {
                var p = context.ConsumptionProduct.Find(c.ProductId);
                p.Stock -= 1;
                context.ConsumptionProduct.Update(p);
            }
            context.MaterialInput.Remove(materialInput);
            await context.SaveChangesAsync();
            return Ok("Devolução removida com sucesso.");
        }
    }
}