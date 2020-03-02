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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupController: ControllerBase
    {
        ControlDbContext context;
        public GroupController(ControlDbContext context) { this.context = context; }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var group = JsonConvert.DeserializeObject<Group>(raw);

                context.Groups.Add(group);
                await context.SaveChangesAsync();
                return Ok("Adicionado com sucesso.");
            }
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var group = JsonConvert.DeserializeObject<Group>(raw);
                group.Id = id;
                context.Groups.Update(group);
                await context.SaveChangesAsync();
                return Ok("Atualizado com sucesso.");
            }
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            using (context)
            {
                var ArrayGroup = context.Groups.ToArray();
                var tojson = JsonConvert.SerializeObject(ArrayGroup);
                return Ok(tojson);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            using (context)
            {
                var group = context.Groups.FirstOrDefault(x => x.Id == id);
                if (group != null)
                {
                    var tojson = JsonConvert.SerializeObject(group);
                    return Ok(tojson);
                }
                else
                {
                    return BadRequest($"O registro com o id {id} não existe.");
                }
            }
        }
        //Remove by id
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var id = JsonConvert.DeserializeObject<int>(raw);
                var obj = context.Groups.FirstOrDefault(x => x.Id == id);

                context.Groups.Remove(obj);
                await context.SaveChangesAsync();
                return Ok("Removido com sucesso.");
            }
        }
    }
}
