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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GeneralProductController : ControllerBase
    {
        ControlDbContext context;
        public GeneralProductController(ControlDbContext context) { this.context = context; }

        //Add new product content every information about
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            using (context)
            {
                var raw = await Helper.RawFromBody(this);
                AboutProduct newProduct = new AboutProduct(raw);
                context.AboutProducts.Add(newProduct);
                await context.SaveChangesAsync();
                return Ok("Produto adicionado com sucesso.");
            }
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var aboutProduct = JsonConvert.DeserializeObject<AboutProduct>(raw);
            aboutProduct.Id = id;
            context.AboutProducts.Update(aboutProduct);
            await context.SaveChangesAsync();
            return Ok("Atualizado com sucesso.");
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var ArrayAboutProducts = context.AboutProducts.ToArray();
            var tojson = JsonConvert.SerializeObject(ArrayAboutProducts);
            return Ok(tojson);
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
            var strid = await Helper.RawFromBody(this);
            int id = int.Parse(strid);
            AboutProduct product = context.AboutProducts.Find(id);
            context.AboutProducts.Remove(product);
            await context.SaveChangesAsync();
            return Ok("Produto removido com sucesso.");
        }

        private AboutProduct GetProductById(int id)
        {
            using (context)
            {
                return context.AboutProducts.FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
