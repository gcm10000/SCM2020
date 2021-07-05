using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using SCM2020___Server.Extensions;
using System;
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
        UserManager<ApplicationUser> userManager;
        public InputController(ControlDbContext context, UserManager<ApplicationUser> userManager) { this.context = context; this.userManager = userManager; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var list = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var list = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);
            return Ok(list);
        }
        [HttpGet("ExistsInput/{invoice}")]
        public IActionResult ShowById(string invoice)
        {
            invoice = System.Uri.UnescapeDataString(invoice);
            var result = context.MaterialInputByVendor.Any(x => x.Invoice == invoice);
            return Ok(result);
        }
        [HttpGet("Invoice/{invoice}")]
        public IActionResult ShowByInvoice(string invoice)
        {
            invoice = System.Uri.UnescapeDataString(invoice);
            invoice = invoice.Trim();
            if (string.IsNullOrWhiteSpace(invoice))
                return Ok();
            var record = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Invoice == invoice);
            return Ok(record);
        }
        [HttpGet("Date/{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        public IActionResult ShowByDate(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        {
            DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay, 0, 0, 0);
            DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay, 23, 59, 59);
            List<AuxiliarConsumption> inputs = new List<AuxiliarConsumption>();

            var listMaterialInput = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts);

            foreach (var input in listMaterialInput)
            {
                var ProductsOfInput = input.ConsumptionProducts;
                var query = ProductsOfInput.Where(t => (t.Date >= dateStart) && (t.Date <= dateEnd));
                foreach (var item in query)
                {
                    //onde tem esse produto em listMaterialInput, resgate a nota fiscal
                    item.WorkOrder = input.Invoice;
                }
                inputs.AddRange(query);
            }

            return Ok(inputs);
        }
        [HttpPost("Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var raw = await Helper.RawFromBody(this);
            var deserialized = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            //var SCMId = userManager.FindByPJERJRegistrationAsync(deserialized.SCMEmployeeId).Id;
            MaterialInputByVendor input = new MaterialInputByVendor(raw);
            context.MaterialInputByVendor.Add(input);
            await context.SaveChangesAsync();
            return Ok("Migração feita com sucesso.");
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var token = Helper.GetToken(this);
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            var input = new MaterialInputByVendor(raw, userId);
            if (context.MaterialInputByVendor.Any(x => x.Invoice == input.Invoice))
                return BadRequest("Já existe uma entrada com esta nota fiscal. Caso queria adicionar um novo produto nesta nota fiscal, atualize a entrada.");

            if (!context.Vendors.Any(x => x.Id == input.VendorId))
                return BadRequest("Fornecedor com ID inexistente.");

            if (!input.ConsumptionProducts.All(x => context.ConsumptionProduct.Any(y => y.Id == x.ProductId)))
                return BadRequest("Há algum produto na lista não cadastrado. Verifique e tente novamente.");

            //Incrementar um produto
            foreach (var item in input.ConsumptionProducts)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock += item.Quantity;
                context.ConsumptionProduct.Update(product);
            }

            foreach (var pp in input.PermanentProducts)
            {
                //Cria um novo produto permanente
                PermanentProduct p = new PermanentProduct()
                {
                    DateAdd = pp.Date,
                    InformationProduct = pp.ProductId,
                    Patrimony = pp.Patrimony,
                    Status = Status.New,
                    WorkOrder = null
                };
                var c = context.ConsumptionProduct.Find(p.InformationProduct);
                p.WorkOrder = null;
                c.Stock += 1;
                context.PermanentProduct.Add(p);
                context.ConsumptionProduct.Update(c);
                await context.SaveChangesAsync();
                
                //Atrela a movimentação
                context.Entry(p).GetDatabaseValues();
                pp.ProductId = p.Id;
            }

            context.MaterialInputByVendor.Add(input);
            await context.SaveChangesAsync();
            return Ok("Entrada por fornecedor foi adicionada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var inputFromJson = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            var oldInput = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList().Single(x => x.Id == id);
            List<AuxiliarConsumption> AuxProducts = new List<AuxiliarConsumption>();
            AuxProducts.AddRange(oldInput.ConsumptionProducts);
            var lPermanent = new List<AuxiliarPermanentInputByVendor>();
            //EDITAR UPDATE PARA ENTRADA DE MATERIAIS PERMANENTES
            //if (.PermanentProducts != null)
            //    lPermanent.AddRange(devolution.PermanentProducts);

            var input = oldInput;
            input.Invoice = inputFromJson.Invoice;
            input.MovingDate = inputFromJson.MovingDate;
            input.VendorId = inputFromJson.VendorId;
            input.ConsumptionProducts = inputFromJson.ConsumptionProducts;

            List<ConsumptionProduct> listProduct = null;
            bool allEquals = input.ConsumptionProducts.All(x => AuxProducts
            .Any(y => (x.Date == y.Date) && (x.ProductId == y.ProductId) && (x.Quantity == y.Quantity)));
            if (!allEquals)
            {
                listProduct = new List<ConsumptionProduct>();
                List<int> ConsumpterProductIds = new List<int>();
                List<int> PermanentsProductIds = new List<int>();
                
                foreach (var p in input.ConsumptionProducts)
                {
                    if (!ConsumpterProductIds.Contains(p.ProductId))
                        ConsumpterProductIds.Add(p.ProductId);
                }

                foreach (var p in input.PermanentProducts)
                {
                    if (!PermanentsProductIds.Contains(p.ProductId))
                        PermanentsProductIds.Add(p.ProductId);
                }

                foreach (var currentId in ConsumpterProductIds)
                {
                    var products = AuxProducts.Where(x => x.ProductId == currentId);
                    double quantityProduct = 0d;
                    foreach (var p in products)
                    {
                        quantityProduct += p.Quantity;
                    }
                    double quantityNewProduct = 0d;
                    var newProducts = input.ConsumptionProducts.Where(x => x.ProductId == currentId);
                    foreach (var p in newProducts)
                    {
                        quantityNewProduct += p.Quantity;
                    }
                    var productModify = context.ConsumptionProduct.Find(currentId);
                    productModify.Stock += (quantityNewProduct - quantityProduct);
                    listProduct.Add(productModify);
                    //context.ConsumptionProduct.Update(productModify);
                }

                ////permanent
                //foreach (var currentId in PermanentsProductIds)
                //{
                //    //oldder - newest
                //    var productModify = await context.ConsumptionProduct.FindAsync(currentId);
                //    double oldder = lPermanent.Count(x => x.ProductId == currentId);
                //    double newest = devolution.PermanentProducts.Count(x => x.ProductId == currentId);
                //    if (oldder != newest)
                //    {
                //        productModify.Stock += newest - oldder;
                //        context.ConsumptionProduct.Update(productModify);
                //    }

                //}
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
            var input = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).SingleOrDefault(x => x.Id == id);
             foreach (var item in input.ConsumptionProducts)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock -= item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            context.MaterialInputByVendor.Remove(input);
            await context.SaveChangesAsync();
            return Ok($"Entrada removida com sucesso.{Environment.NewLine}{input.ConsumptionProducts.Count} produto(s) foram descontados no sistema.");
        }


    }
}
