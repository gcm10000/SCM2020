using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCM2020___Server;
using SCM2020___Server.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.SCM)]
    public class ProductController : ControllerBase
    {
        ControlDbContext context;
        public ProductController(ControlDbContext context)
        {
            this.context = context;
        }
        //Add new product content every information about
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                PermanentProduct newProduct = new PermanentProduct(raw);
                context.IndividualProducts.Add(newProduct);
                await context.SaveChangesAsync();
                return Ok("Produto adicionado com sucesso.");
            }
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                var product = context.IndividualProducts.FirstOrDefault(x => x.Id == id);

                var productparsed = JObject.Parse(raw);
                var infoId = productparsed.Value<int>("InformationProduct");
                product.InformationProduct = infoId;
                context.IndividualProducts.Update(product);
                await context.SaveChangesAsync();
                return Ok("Atualizado com sucesso.");
            }
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            using (context)
            {
                var tojson = JsonConvert.SerializeObject(context.IndividualProducts.ToArray());
                return Ok(tojson);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            using (context)
            {
                var product = context.AboutProducts.FirstOrDefault(x => x.Id == id);
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
        }

        //Remove by ID
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove()
        {
            using (context)
            {
                var strid = await Helper.RawFromBody(this);
                int id = int.Parse(strid);
                ConsumptionProduct product = context.AboutProducts.Find(id);
                context.AboutProducts.Remove(product);
                await context.SaveChangesAsync();
                return Ok("Produto removido com sucesso.");
            }
        }
    }
}
