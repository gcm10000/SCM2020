﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = Roles.SCM)]
    public class DevolutionController : ControllerBase
    {
        ControlDbContext context;
        public DevolutionController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            return Ok(context.MaterialInput.ToList());
        }
        [HttpPost("New")]
        public async Task<IActionResult> DevolutionConsumpterProduct()
        {
            var raw = await Helper.RawFromBody(this);
            MaterialInput materialInput = new MaterialInput(raw);
            if (context.MaterialOutput.Any(x => x.WorkOrder == materialInput.WorkOrder))
            {
                var output = context.MaterialOutput.SingleOrDefault(x => x.WorkOrder == materialInput.WorkOrder);
                //if (materialInput.ConsumptionProducts.All(x => output.ConsumptionProducts.Any(y => x == y)))
                //{

                //}
                context.MaterialInput.Add(materialInput);
                await context.SaveChangesAsync();
                return Ok("Produto adicionado com sucesso.");
            }
            return Ok("");
        }
    }
}
