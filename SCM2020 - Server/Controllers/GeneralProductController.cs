using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GeneralProductController : ControllerBase
    {
        ControlDbContext context;
        public GeneralProductController(ControlDbContext context) { this.context = context; }

        //Add new consumpter product content every information about
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                ConsumptionProduct newProduct = new ConsumptionProduct(raw);

                //if (!context.Vendors.Any(x => x.Id == newProduct.Vendor))
                //    return BadRequest("Não existe fornecedor com este id.");
                if (!context.Groups.Any(x => x.Id == newProduct.Group))
                    return BadRequest("Não existe grupo com este id.");

                context.ConsumptionProduct.Add(newProduct);
                await context.SaveChangesAsync();
                return Ok("Produto adicionado com sucesso.");
            }
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var consumptionProduct = JsonConvert.DeserializeObject<ConsumptionProduct>(raw);
            consumptionProduct.Id = id;

            //if (!context.Vendors.Any(x => x.Id == consumptionProduct.Vendor))
            //    return BadRequest("Não existe fornecedor com este id.");
            if (!context.Groups.Any(x => x.Id == consumptionProduct.Group))
                return BadRequest("Não existe grupo com este id.");

            context.ConsumptionProduct.Update(consumptionProduct);
            await context.SaveChangesAsync();
            return Ok("Atualizado com sucesso.");
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var ArrayInfoProducts = context.ConsumptionProduct.ToArray();
            var tojson = JsonConvert.SerializeObject(ArrayInfoProducts);
            return Ok(tojson);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ShowById(int id)
        {
            var product = await context.ConsumptionProduct.SingleOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                var tojson = JsonConvert.SerializeObject(product);
                return Ok(tojson);
            }
            else
            {
                return BadRequest($"O registro com o id {id} não existe.");
            }
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> ShowByCode(int code)
        {
            var product = await context.ConsumptionProduct.SingleOrDefaultAsync(x => x.Code == code);
            if (product != null)
            {
                var tojson = JsonConvert.SerializeObject(product);
                return Ok(tojson);
            }
            else
            {
                return BadRequest($"O registro com o código {code} não existe.");
            }
        }

        //Remove by ID
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove()
        {
            var strid = await Helper.RawFromBody(this);
            int id = int.Parse(strid);
            ConsumptionProduct product = context.ConsumptionProduct.Find(id);
            context.ConsumptionProduct.Remove(product);
            await context.SaveChangesAsync();
            return Ok("Produto removido com sucesso.");
        }
    }
}
