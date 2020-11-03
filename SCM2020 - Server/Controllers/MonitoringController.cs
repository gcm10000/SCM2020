using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using SCM2020___Server.Extensions;
using System;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Cryptography.X509Certificates;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class MonitoringController : ControllerBase
    {
        ControlDbContext context;
        UserManager<ApplicationUser> userManager;
        public MonitoringController(UserManager<ApplicationUser> userManager, ControlDbContext context) { this.userManager = userManager; this.context = context; }

        [HttpGet]
        public IActionResult ShowAll()
        {
            var list = context.Monitoring.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var monitoring = context.Monitoring.Find(id);
            return Ok(monitoring);
        }
        [HttpGet("WorkOrder/{workorder}")]
        public ActionResult<Monitoring> ShowByWorkOrder(string workorder)
        {
            workorder = System.Uri.UnescapeDataString(workorder);
            var monitoring = context.Monitoring.SingleOrDefault(x => x.Work_Order == workorder);
            return monitoring;
        }
        [HttpGet("CheckWorkOrder/{workorder}")]
        public IActionResult CheckWorkOrder(string workorder)
        {
            workorder = System.Uri.UnescapeDataString(workorder);
            var result = context.Monitoring.SingleOrDefault(x => x.Work_Order == workorder);
            bool situation = false;
            if (result != null)
                situation = result.Situation;
            return Ok(situation);
        }
        //[Authorize(Roles = Roles.Administrator)]
        [AllowAnonymous]
        [HttpPost("Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var raw = await Helper.RawFromBody(this);
            var deserialized = JsonConvert.DeserializeObject<Monitoring>(raw);
            var SCMId = userManager.FindByRegister(deserialized.SCMEmployeeId).Id;
            Monitoring monitoring = new Monitoring(raw);
            //NOME DO FUNCIONÁRIO
            monitoring.SCMEmployeeId = SCMId;
            //Sector sector = context.Sectors.Single(x => x.NumberSector == int.Parse(monitoring.Work_Order.Substring(2)));
            if (int.TryParse(monitoring.Work_Order.Substring(0, 2), out int result))
            {
                if (context.Sectors.Any(x => x.NumberSector == result))
                {
                    monitoring.SectorId = context.Sectors.Single(x => x.NumberSector == result).Id;
                }
                else
                {
                    monitoring.SectorId = context.Sectors.Single(x => x.NameSector == "DETEL").Id;
                }
            }
            var UserId = userManager.FindByFullName(deserialized.EmployeeId);
            monitoring.EmployeeId = (UserId).Id;
            context.Monitoring.Add(monitoring);
            await context.SaveChangesAsync();
            return Ok("Migração de dados feita com sucesso.");
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Create()
        {
            var token = Helper.GetToken(this);
            var unique = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            var user = userManager.Users.SingleOrDefault(u => u.Id == unique);
            var monitoring = new Monitoring(raw, user.Id);
            if (!userManager.Users.Any(x => x.Id == monitoring.EmployeeId))
                return BadRequest("Funcionário do qual solicitou não está cadastrado.");
            if (!context.Sectors.Any(x => x.Id == monitoring.RequestingSector))
                return BadRequest("Setor não cadastrado.");
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
            var monitoringFromJson = JsonConvert.DeserializeObject<Monitoring>(raw);
            var monitoring = context.Monitoring.Find(id);
            monitoring.ClosingDate = monitoringFromJson.ClosingDate;
            monitoring.EmployeeId = monitoringFromJson.EmployeeId;
            monitoring.MovingDate = monitoringFromJson.MovingDate;
            //monitoring.SCMEmployeeId = monitoringFromJson.SCMEmployeeId;
            //monitoring.Situation = monitoringFromJson.Situation;
            monitoring.Work_Order = monitoringFromJson.Work_Order;
            
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento atualizada com sucesso.");
        }
        [HttpPost("Closure/{year}/{month}/{day}")]
        public async Task<IActionResult> Closure(int year, int month, int day)
        {
            var raw = await Helper.RawFromBody(this);
            var workOrder = JsonConvert.DeserializeObject<string>(raw);

            var monitoring = context.Monitoring.Single(x => x.Work_Order == workOrder);
            DateTime dateTime = new DateTime(year, month, day);
            monitoring.Situation = true;
            monitoring.ClosingDate = dateTime;
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento atualizada com sucesso.");
        }        
        [HttpGet("Reopen/{workOrder}")]
        public async Task<IActionResult> Reopen(string workOrder)
        {
            workOrder = System.Uri.UnescapeDataString(workOrder);

            var monitoring = context.Monitoring.Single(x => x.Work_Order == workOrder);
            monitoring.Situation = false;
            monitoring.ClosingDate = null;
            context.Monitoring.Update(monitoring);
            await context.SaveChangesAsync();
            return Ok("Ordem de serviço aberta com sucesso.");
        }
        [HttpGet("SearchByDate/{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        public IActionResult ShowByDate(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        {
            DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay);
            DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay);

            var result = context.Monitoring.Where(t => (t.MovingDate >= dateStart) && (t.MovingDate <= dateEnd));

            return Ok(result);
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            //Checa monivomentações referente a ordem de serviço
            var monitoring = context.Monitoring.Find(id);
            if ((context.MaterialOutput.Any(x => x.WorkOrder == monitoring.Work_Order)) || context.MaterialInput.Any(x => x.WorkOrder == monitoring.Work_Order))
                return BadRequest("Já contém movimentações nesta ordem de serviço. Para remover o monitoramento, remova as movimentações.");
            context.Monitoring.Remove(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento removida com sucesso.");
        }
    }
}
