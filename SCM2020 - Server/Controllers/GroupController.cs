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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupController: ControllerBase
    {
        ControlDbContext context;
        public GroupController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var lGroup = context.Groups.ToList();
            return Ok(lGroup);
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var group = context.Groups.FirstOrDefault(x => x.Id == id);
            if (group == null)
                return BadRequest($"O registro com o id {id} não existe.");
            return Ok(group);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            var group = JsonConvert.DeserializeObject<Group>(raw);

            context.Groups.Add(group);
            await context.SaveChangesAsync();
            return Ok("Adicionado com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var group = JsonConvert.DeserializeObject<Group>(raw);
            group.Id = id;
            context.Groups.Update(group);
            await context.SaveChangesAsync();
            return Ok("Atualizado com sucesso.");
        }
        //Remove by id
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove()
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
