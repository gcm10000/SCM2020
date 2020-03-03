using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = Roles.SCM)]
    public class DevolutionController : ControllerBase
    {
        ControlDbContext context;
        public DevolutionController(ControlDbContext context) { this.context = context; }

        [HttpPost("")]
        public async Task<IActionResult> DevolutionOfConsumpterProduct()
        {
            var raw = await Helper.RawFromBody(this);
            //MaterialInput materialInput = new MaterialInput();
            //materialInput.DocDate = DateTime.Now;;
            //materialInput.EmployeeId = 1;
            //materialInput.Id = 1;
            //materialInput.MovingDate = DateTime.Now;
            //materialInput.Regarding = Regarding.InternalTransfer;
            //materialInput.SCMEmployeeId = 15;
            //materialInput.WorkOrder = "400662/13";
            //materialInput.
            //var newMaterial = ;
            //context.MaterialInput.Add(raw);
            return Ok();
        }
    }
}
