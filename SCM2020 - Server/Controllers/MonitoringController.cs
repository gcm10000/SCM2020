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
        public async Task<ActionResult> ShowAll()
        {
            var list = await context.Monitoring.ToListAsync();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> ShowById(int id)
        {
            var list = await context.Monitoring.FindAsync(id);
            return Ok(list);
        }
        [HttpPost("Add")]
        public async Task<ActionResult> Create()
        {
            var raw = await Helper.RawFromBody(this);
            var monitoring = new Monitoring(raw);
            context.Monitoring.Add(monitoring);
            await context.SaveChangesAsync();
            return Ok("Movimentação adicionada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<ActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var monitoring = new Monitoring(raw);
            monitoring.Id = id;
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Movimentação atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            //Check output with these order work
            //if ()...
            var monitoring = await context.Monitoring.FindAsync(id);
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Movimentação removida com sucesso.");
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
