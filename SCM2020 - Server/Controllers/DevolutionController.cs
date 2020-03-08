using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Context;
using ModelsLibrary;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.SCM)]
    public class DevolutionController : ControllerBase
    {
        ControlDbContext context;
        public DevolutionController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var devolution = context.MaterialInput.ToList();
            return Ok(devolution);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var devolution = context.MaterialInput.Find(id);
            return Ok(devolution);
        }
        [HttpGet("WorkOrder/{workorder}")]
        public IActionResult ShowByWorkOrder(string workorder)
        {
            var devolution = context.MaterialInput.SingleOrDefault(x => x.WorkOrder == workorder);
            return Ok(devolution);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            MaterialInput materialInput = new MaterialInput(raw);
            var output = context.MaterialOutput.FirstOrDefault(x => x.WorkOrder == materialInput.WorkOrder);
            if (output == null)
                return BadRequest("Não há qualquer saída existente nessa ordem de serviço.");
            bool allMatches = materialInput.ConsumptionProducts
                .All(x => output.ConsumptionProducts
                .Any(y => y.Id == x.ProductId));
            if (!allMatches)
                return BadRequest("Existem itens que estão na entrada e que não pertencem a saída.");

            context.MaterialInput.Add(materialInput);
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
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove()
        {
            var raw = await Helper.RawFromBody(this);
            int id = int.Parse(raw);
            var materialInput = context.MaterialInput.Find(id);
            if (context.Monitoring.Any(x => (x.Work_Order == materialInput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");

            context.MaterialInput.Remove(materialInput);
            await context.SaveChangesAsync();
            return Ok("Devolução removida com sucesso.");
        }
    }
}