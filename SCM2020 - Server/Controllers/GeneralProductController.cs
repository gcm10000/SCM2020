using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using ModelsLibraryCore;
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

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost("Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var raw = await Helper.RawFromBody(this);
            ConsumptionProduct product = new ConsumptionProduct(raw);
            context.ConsumptionProduct.Add(product);
            await context.SaveChangesAsync();
            return Ok("Migração feita com sucesso.");
        }
        //Add new consumpter product content every information about
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ShowAll()
        {
            var lProduct = context.ConsumptionProduct.ToList();
            return Ok(lProduct);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ShowById(int id)
        {
            var product = await context.ConsumptionProduct.SingleOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"O registro com o id {id} não existe.");
            }
        }
        [AllowAnonymous]
        [HttpGet("Code/{code}")]
        public IActionResult ShowByCode(int code)
        {
            var product = context.ConsumptionProduct.SingleOrDefault(x => x.Code == code);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"O registro com o código {code} não existe.");
            }
        }
        [AllowAnonymous]
        [HttpGet("Search/{query}")]
        public IActionResult Search(string query)
        {
            string[] querySplited = query.Split(' ');

            var lproduct = context.ConsumptionProduct.ToList()
                .Where(x => x.Description.RemoveDiacritics().MultiplesContains(querySplited) || x.Code.ToString().Contains(query));
            if (int.TryParse(query, out int result))
            {
                lproduct = lproduct.AsEnumerable()
                        .OrderBy(x => !x.Code.ToString().Contains(query))
                        .ToList();
            }
            return Ok(lproduct);
        }
        //Show information today about products
        [HttpGet("Inventory")]
        public IActionResult Inventory()
        {
            var ArrayInfoProducts = context.ConsumptionProduct.ToList();
            var listInventory = new List<object>();
            foreach (var product in ArrayInfoProducts)
            {
                listInventory.Add(new
                {
                    Code = product.Code,
                    Description = product.Description,
                    Stock = product.Stock,
                });
            }
            //var tojson = JsonConvert.SerializeObject(listInventory, Formatting.Indented);

            return Ok(listInventory);
        }
        //Remove by ID
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            ConsumptionProduct product = context.ConsumptionProduct.Find(id);
            context.ConsumptionProduct.Remove(product);
            await context.SaveChangesAsync();
            return Ok("Produto removido com sucesso.");
        }
    }
}
