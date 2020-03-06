using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class OutputController : ControllerBase
    {
        ControlDbContext context;
        public OutputController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult Show()
        {
            var lOutput = context.MaterialOutput.ToList();
            return Ok(lOutput);
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var lOutput = context.MaterialOutput.Find(id);
            return Ok(lOutput);
        }
        [HttpGet("{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        public IActionResult ShowByDate(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        {
            DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay);
            DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay);
            List<AuxiliarConsumption> outputs = new List<AuxiliarConsumption>();
            foreach (var output in context.MaterialOutput.ToList())
            {
                outputs.AddRange(output.ConsumptionProducts.Where(t => (t.Date >= dateStart) && (t.Date <= dateEnd)));
            }

            return Ok(outputs.ToList());
        }
        [HttpPost("New")]
        public async Task<IActionResult> NewOutput()
        {
            var raw = await Helper.RawFromBody(this);
            var output = new MaterialOutput(raw);
            var monitoring = context.Monitoring.SingleOrDefault(x => x.Work_Order == output.WorkOrder);
            if (monitoring == null)
                return BadRequest("Ordem de serviço inexistente.");
            if (monitoring.Situation == true)
                return BadRequest("Ordem de serviço fechada.");
            var arrayConsumption = output.ConsumptionProducts.ToList();
            var arrayPermanent = output.PermanentProducts.ToList();

            //Check if every objects of ConsumptionProduct (less) are inside list ConsumptionProduct (bigger)
            bool MatchesConsumption = arrayConsumption
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.ProductId));
            bool MatchesPermanent = arrayPermanent
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.ProductId));
            if (!MatchesConsumption)
                return BadRequest("Há produtos de consumo não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");
            if (!MatchesPermanent)
                return BadRequest("Há produtos permanentes não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");

            context.MaterialOutput.Add(output);

            arrayConsumption.ForEach(x => context.ConsumptionProduct.Find(x.ProductId).Stock -= 1);
            arrayPermanent.ForEach(x => context.ConsumptionProduct.Find(x.ProductId).Stock -= 1);

            await context.SaveChangesAsync();
            return Ok(JsonConvert.SerializeObject(output));
        }
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var outputFromRaw = JsonConvert.DeserializeObject<MaterialOutput>(raw);
            outputFromRaw.Id = id;
            
            context.MaterialOutput.Update(outputFromRaw);
            await context.SaveChangesAsync();
            return Ok("Movimentação de saída atualizada com sucesso.");
        }
    }
}
