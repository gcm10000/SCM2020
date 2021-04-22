using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GeneralProductController : ControllerBase
    {
        ControlDbContext context;
        IHostingEnvironment _env;
        public GeneralProductController(ControlDbContext context, IHostingEnvironment env) { this.context = context;  this._env = env; }

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

            if (context.ConsumptionProduct.Any(x => x.Code == newProduct.Code))
                return BadRequest("Código já utilizado.");
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
        [HttpGet]
        public IActionResult ShowAll()
        {
            var lProduct = context.ConsumptionProduct.ToList();
            return Ok(lProduct);
        }
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
        [HttpGet("Search/{query}")]
        public IActionResult Search(string query)
        {
            query = System.Uri.UnescapeDataString(query);

            if (string.IsNullOrWhiteSpace(query))
                return Ok();

            string[] querySplited = query.Trim().Split(' ');
            
            var firstNumber = querySplited.FirstOrDefault(x => x.IsDigitsOnly());

            IEnumerable<ConsumptionProduct> lproduct = null;
            if ((firstNumber != null) && (querySplited.Length > 1))
            {
                lproduct = context.ConsumptionProduct.ToList()
                    .Where(x => string.Join(" ", x.Description, x.Code).MultiplesContainsWords(querySplited));
            }
            else
            {
                lproduct = context.ConsumptionProduct.ToList()
                    .Where(x => x.Description.MultiplesContainsWords(querySplited) || x.Code.ToString().Contains(query));
            }

            if (query.IsDigitsOnly())
            {
                lproduct = lproduct.AsEnumerable()
                        .OrderBy(x => x.Code)
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
        [HttpGet("NextNumber")]
        public int NextNumber()
        {
            var numbers = context.ConsumptionProduct.Select(x => x.Code);
            int nextNumber = numbers.NextAvaliable();
            return nextNumber;
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
        [HttpPost("UploadImage")]
        public async Task<IActionResult> OnPostUploadAsync([FromForm] ImageInput imageInput)
        {
            if (imageInput.Image.Length < 10485760)
            {
                //string path = Path.Combine("img", imageInput.Id.ToString() + Path.GetExtension(imageInput.Image.FileName));
                string relativeUrl = Helper.Combine("img", imageInput.Id.ToString() + Path.GetExtension(imageInput.Image.FileName));
                var product = context.ConsumptionProduct.Find(imageInput.Id);
                product.Photo = relativeUrl;
                string fullName = Path.Combine(_env.WebRootPath, relativeUrl);
                
                using (var stream = System.IO.File.Create(fullName))
                {
                    using (var ms = new MemoryStream())
                    {
                        await imageInput.Image.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        //Averiguar se é uma imagem válida
                        if (!((Helper.GetImageFormat(fileBytes) == ImageFormat.tiff) || (Helper.GetImageFormat(fileBytes) == ImageFormat.unknown)))
                        {
                            await imageInput.Image.CopyToAsync(stream);
                        }
                        else
                        {
                            return BadRequest("Este arquivo não é uma imagem ou não é um formato compatível.");
                        }
                    }

                }
                await context.SaveChangesAsync();
                return Ok("Imagem enviada com sucesso.");
            }
            else
            {
                return BadRequest("Imagem maior ou igual a 10 MB. Envie um tamanho menor.");
            }
        }
    }
}
