using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class MonitoringController : ControllerBase
    {
        ControlDbContext context;
        public MonitoringController(ControlDbContext context) { this.context = context; }

        [HttpGet]
        public IActionResult ShowAll()
        {
            var list = context.Monitoring.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.Monitoring.Find(id);
            return Ok(list);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Create()
        {
            var raw = await Helper.RawFromBody(this);
            var monitoring = new Monitoring(raw);
            if (context.Monitoring.Any(x => x.Work_Order == monitoring.Work_Order))
                return BadRequest("Ordem de serviço já existente.");
            context.Monitoring.Add(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento adicionada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var monitoring = new Monitoring(raw);
            monitoring.Id = id;
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            //Check output with these order work
            var monitoring = context.Monitoring.Find(id);
            if (context.MaterialOutput.Any(x => x.WorkOrder == monitoring.Work_Order))
                return BadRequest("Já contém monitoramento nesta ordem de serviço. Para remover o monitoramento, remova as movimentações.");
            context.Monitoring.Remove(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento removida com sucesso.");
        }

        //[HttpGet("{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        //public async Task<ActionResult> ShowBeetweenTwoDates(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        //{
        //    DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay);
        //    DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay);
        //    var list = context.Monitoring.ToList().Where(t => t.ClosingDate >= dateStart && t.);
        //    return Ok(list);
        //}

    }
}
