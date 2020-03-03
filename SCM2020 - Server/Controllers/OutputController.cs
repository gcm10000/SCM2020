using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using SCM2020___Server.Models;
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
        [HttpPost("New")]
        public async Task<IActionResult> NewOutput()
        {
            var raw = await Helper.RawFromBody(this);
            var output = new MaterialOutput(raw);
            context.MaterialOutput.Add(output);
            await context.SaveChangesAsync();
            return Ok(JsonConvert.SerializeObject(output));
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var output = JsonConvert.DeserializeObject<MaterialOutput>(raw);
            output.Id = id;
            context.MaterialOutput.Update(output);
            await context.SaveChangesAsync();
            return Ok("Movimentação de saída atualizada com sucesso.");
        }

    }
}
