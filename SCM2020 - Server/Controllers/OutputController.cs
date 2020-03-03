using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using SCM2020___Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class OutputController : ControllerBase
    {
        ControlDbContext context;
        public OutputController(ControlDbContext context) { this.context = context; }
        [HttpPost("New")]
        public async Task<IActionResult> NewOutput()
        {
            var raw = await Helper.RawFromBody(this);
            var output = new MaterialOutput(raw);
            var monitoring = context.Monitoring.SingleOrDefault(x => x.Work_Order == output.WorkOrder);
            if (monitoring == null)
                return BadRequest("Ordem de serviço inexistente.");
            if (monitoring.Situation == false)
                return BadRequest("Ordem de serviço fechada.");
            var arrayConsumption = output.ConsumptionProducts.ToList();
            var arrayPermanent = output.PermanentProducts.ToList();

            //Check if every objects of ConsumptionProduct (less) are inside list ConsumptionProduct (bigger)
            bool MatchesConsumption = arrayConsumption
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.ConsumperId));
            bool MatchesPermanent = arrayPermanent
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.PermanentId));
            if (!MatchesConsumption)
                return BadRequest("Há produtos de consumo não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");
            if (!MatchesPermanent)
                return BadRequest("Há produtos permanentes não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");

            context.MaterialOutput.Add(output);

            arrayConsumption.ForEach(x => context.ConsumptionProduct.Find(x.ConsumperId).Stock -= 1);
            arrayPermanent.ForEach(x => context.ConsumptionProduct.Find(x.PermanentId).Stock -= 1);

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
