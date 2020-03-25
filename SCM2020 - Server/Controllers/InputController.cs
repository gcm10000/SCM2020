using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class InputController : ControllerBase
    {
        ControlDbContext context;
        public InputController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var list = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).SingleOrDefault(x => x.Id == id);
            return Ok(list);
        }
        [HttpGet("Invoice/{invoice}")]
        public IActionResult ShowByInvoice(string invoice)
        {
            var record = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).SingleOrDefault(x => x.Invoice == invoice);
            return Ok(record);
        }
        
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            bool b = Helper.GetToken(out System.IdentityModel.Tokens.Jwt.JwtSecurityToken token, this);
            if (!b)
                return BadRequest("Por favor, faça login.");
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            var input = new MaterialInputByVendor(raw, id);
            if (context.MaterialInputByVendor.Any(x => x.Invoice == input.Invoice))
                return BadRequest("Já existe uma entrada com esta nota fiscal. Caso queria adicionar um novo produto nesta nota fiscal, atualize a entrada.");

            if (!input.AuxiliarConsumptions.All(x => context.ConsumptionProduct.Any(y => y.Id == x.ProductId)))
                return BadRequest("Há algum produto na lista não cadastrado. Verifique e tente novamente.");
            context.MaterialInputByVendor.Add(input);
            //Incrementar um produto
            foreach (var item in input.AuxiliarConsumptions)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock += item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            await context.SaveChangesAsync();
            return Ok("Entrada por fornecedor foi adicionada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var inputFromJson = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            var oldInput = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).ToList().Single(x => x.Id == id);
            List<AuxiliarConsumption> AuxProducts = new List<AuxiliarConsumption>();
            AuxProducts.AddRange(oldInput.AuxiliarConsumptions);

            var input = oldInput;
            input.Invoice = inputFromJson.Invoice;
            input.MovingDate = inputFromJson.MovingDate;
            input.VendorId = inputFromJson.VendorId;
            input.AuxiliarConsumptions = inputFromJson.AuxiliarConsumptions;

            List<ConsumptionProduct> listProduct = null;
            bool allEquals = input.AuxiliarConsumptions.All(x => AuxProducts
            .Any(y => (x.Date == y.Date) && (x.ProductId == y.ProductId) && (x.Quantity == y.Quantity)));
            if (!allEquals)
            {
                listProduct = new List<ConsumptionProduct>();
                List<int> productIds = new List<int>();
                foreach (var p in input.AuxiliarConsumptions)
                {
                    if (!productIds.Contains(p.ProductId))
                        productIds.Add(p.ProductId);
                }

                foreach (var currentId in productIds)
                {
                    var products = AuxProducts.Where(x => x.ProductId == currentId);
                    double quantityProduct = 0d;
                    foreach (var p in products)
                    {
                        quantityProduct += p.Quantity;
                    }
                    double quantityNewProduct = 0d;
                    var newProducts = input.AuxiliarConsumptions.Where(x => x.ProductId == currentId);
                    foreach (var p in newProducts)
                    {
                        quantityNewProduct += p.Quantity;
                    }
                    var productModify = context.ConsumptionProduct.Find(currentId);
                    productModify.Stock += (quantityNewProduct - quantityProduct);
                    listProduct.Add(productModify);
                    //context.ConsumptionProduct.Update(productModify);
                }
            }
            if (listProduct != null)
                context.ConsumptionProduct.UpdateRange(listProduct);
            context.MaterialInputByVendor.Update(input);
            await context.SaveChangesAsync();
            return Ok("A entrada foi atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var input = context.MaterialInputByVendor.Include(x => x.AuxiliarConsumptions).SingleOrDefault(x => x.Id == id);
            foreach (var item in input.AuxiliarConsumptions)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock -= item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            context.MaterialInputByVendor.Remove(input);
            await context.SaveChangesAsync();
            return Ok($"Entrada foi removida com sucesso.\n{input.AuxiliarConsumptions.Count} produtos foram descontados no sistema.");
        }


    }
}
