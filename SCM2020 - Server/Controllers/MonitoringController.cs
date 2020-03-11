﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult ShowByWorkOrder(string workorder)
        {
            var monitoring = context.Monitoring.SingleOrDefault(x => x.Work_Order == workorder);
            return Ok(monitoring);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Create()
        {
            bool b = Helper.GetToken(out System.IdentityModel.Tokens.Jwt.JwtSecurityToken token, this);
            if (!b)
                return BadRequest("Por favor, faça login.");
            var unique = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            var user = userManager.Users.SingleOrDefault(u => u.Id == unique);
            var monitoring = new Monitoring(raw, user.Id);
            if (!userManager.Users.Any(x => x.Id == monitoring.EmployeeId))
                return BadRequest("Funcionário do qual solicitou não está cadastrado.");
            //salling

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
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            //Check output with these order work
            var monitoring = context.Monitoring.Find(id);
            if (context.MaterialOutput.Any(x => x.WorkOrder == monitoring.Work_Order))
                return BadRequest("Já contém movimentações nesta ordem de serviço. Para remover o monitoramento, remova as movimentações.");
            context.Monitoring.Remove(monitoring);
            await context.SaveChangesAsync();
            return Ok("Monitoramento removida com sucesso.");
        }
    }
}
