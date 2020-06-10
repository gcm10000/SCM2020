using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SCM2020___Server.Extensions;
using System;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class DevolutionController : ControllerBase
    {
        ControlDbContext context;
        UserManager<ApplicationUser> userManager;
        public DevolutionController(ControlDbContext context, UserManager<ApplicationUser> userManager) { this.context = context; this.userManager = userManager; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            return Ok(devolution);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);
            return Ok(devolution);
        }
        [HttpGet("WorkOrder/{workorder}")]
        public IActionResult ShowByWorkOrder(string workorder)
        {
            workorder = System.Uri.UnescapeDataString(workorder);
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.WorkOrder == workorder);
            return Ok(devolution);
        }
        [HttpGet("Date/{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        public IActionResult ShowByDate(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        {
            DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay);
            DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay);
            List<AuxiliarConsumption> inputs = new List<AuxiliarConsumption>();
            foreach (var input in context.MaterialInput.ToList())
            {
                inputs.AddRange(input.ConsumptionProducts.Where(t => (t.Date >= dateStart) && (t.Date <= dateEnd)));
            }

            return Ok(inputs.ToList());
        }
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost("Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var raw = await Helper.RawFromBody(this);
            var deserialized = JsonConvert.DeserializeObject<MaterialInput>(raw);
            //var SCMId = userManager.FindByPJERJRegistrationAsync(deserialized.SCMEmployeeId).Id;
            //MaterialInput materialInput = new MaterialInput(raw, SCMId);
            MaterialInput materialInput = new MaterialInput(raw);
            //materialInput.Monitoring = context.Monitoring.First(x => x.Work_Order == materialInput.WorkOrder);
            //materialInput.EmployeeId = userManager.FindByPJERJRegistrationAsync(deserialized.EmployeeId).Id;
            context.MaterialInput.Add(materialInput);
            await context.SaveChangesAsync();
            return Ok("Migração feita com sucesso.");
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            bool b = Helper.GetToken(out System.IdentityModel.Tokens.Jwt.JwtSecurityToken token, this);
            if (!b)
                return BadRequest("Por favor, faça login.");
            var id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            //MaterialInput materialInput = new MaterialInput(raw, id);
            MaterialInput materialInput = new MaterialInput(raw);
            var output = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts)
                .FirstOrDefault(x => x.WorkOrder == materialInput.WorkOrder);
            if (output == null)
                return BadRequest("Não há qualquer saída existente nessa ordem de serviço.");
            bool allMatches = materialInput.ConsumptionProducts
                .All(x => output.ConsumptionProducts
                .Any(y => y.ProductId == x.ProductId));
            if (!allMatches)
                return BadRequest("Existem itens que estão na entrada e que não pertencem a saída.");
            //var allMatches2 = materialInput.ConsumptionProducts
            //    .Where(x => output.ConsumptionProducts
            //    .Any(y => x.Quantity.CompareTo(y.Quantity) > 0));

            var allMatches2 = new List<AuxiliarConsumption>();
            foreach (var item in materialInput.ConsumptionProducts)
            {
                var y = output.ConsumptionProducts.First(x => x.ProductId == item.ProductId);
                if (item.Quantity.CompareTo(y.Quantity) > 0)
                {
                    if (item.Quantity.CompareTo(y.Quantity) > 0)
                    {
                        allMatches2.Add(item);
                    }
                }
            }
            if (allMatches2.Count() > 0)
            {
                string names = string.Empty;
                foreach (var match in allMatches2)
                {
                    names += context.ConsumptionProduct.Find(match.ProductId).Description + "\n";
                }
                return BadRequest($"O seguintes materiais estão com valor acima da quantidade que foi solicitada na saída:\n{names}");

            }

            context.MaterialInput.Add(materialInput);
            if (materialInput.ConsumptionProducts != null)
                foreach (var item in materialInput.ConsumptionProducts)
                {
                    var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                    product.Stock += item.Quantity;
                    context.ConsumptionProduct.Update(product);
                }

            if (materialInput.PermanentProducts != null)
                foreach (var p in materialInput.PermanentProducts)
                {
                    var c = context.ConsumptionProduct.Find(p.ProductId);
                    c.Stock += 1;
                    context.ConsumptionProduct.Update(c);
                }

            await context.SaveChangesAsync();
            return Ok("Nova devolução registrada com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var devolutionFromJson = JsonConvert.DeserializeObject<MaterialInput>(raw);
            var devolution = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts)
                .SingleOrDefault(x => x.Id == id);
            if (context.Monitoring.Any(x => (x.Work_Order == devolution.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");

            var lConsumpter = new List<AuxiliarConsumption>();
            lConsumpter.AddRange(devolution.ConsumptionProducts);
            var lPermanent = new List<AuxiliarPermanent>();
            if (devolution.PermanentProducts != null)
                lPermanent.AddRange(devolution.PermanentProducts);

            devolution.ConsumptionProducts = devolutionFromJson.ConsumptionProducts;
            devolution.DocDate = devolutionFromJson.DocDate;
            //devolution.EmployeeId = oldMaterialInput.EmployeeId;
            devolution.MovingDate = devolutionFromJson.MovingDate;
            devolution.PermanentProducts = devolutionFromJson.PermanentProducts;
            devolution.Regarding = devolutionFromJson.Regarding;
            //devolution.WorkOrder = oldMaterialInput.WorkOrder;

            //List<ConsumptionProduct> listProduct = new List<ConsumptionProduct>();

            List<int> ConsumpterProductIds = new List<int>();
            List<int> PermanentsProductIds = new List<int>();

            foreach (var p in devolutionFromJson.ConsumptionProducts)
            {
                if (!ConsumpterProductIds.Contains(p.ProductId))
                    ConsumpterProductIds.Add(p.ProductId);
            }
            foreach (var p in devolutionFromJson.PermanentProducts)
            {
                if (!PermanentsProductIds.Contains(p.ProductId))
                    PermanentsProductIds.Add(p.ProductId);
            }

            foreach (var currentId in ConsumpterProductIds)
            {
                var products = lConsumpter.Where(x => x.ProductId == currentId);
                double quantityProduct = 0d;
                foreach (var p in products)
                {
                    quantityProduct += p.Quantity;
                }
                double quantityNewProduct = 0d;
                var newProducts = devolution.ConsumptionProducts.Where(x => x.ProductId == currentId);
                foreach (var p in newProducts)
                {
                    quantityNewProduct += p.Quantity;
                }
                var productModify = context.ConsumptionProduct.Find(currentId);
                productModify.Stock += (quantityNewProduct - quantityProduct);
                //listProduct.Add(productModify);
                context.ConsumptionProduct.Update(productModify);
            }

            //permanent
            foreach (var currentId in PermanentsProductIds)
            {
                //oldder - newest
                var productModify = await context.ConsumptionProduct.FindAsync(currentId);
                double oldder = lPermanent.Count(x => x.ProductId == currentId);
                double newest = devolution.PermanentProducts.Count(x => x.ProductId == currentId);
                if (oldder != newest)
                {
                    productModify.Stock += newest - oldder;
                    context.ConsumptionProduct.Update(productModify);
                }
            }
            context.MaterialInput.Update(devolution);
            await context.SaveChangesAsync();
            return Ok("Devolução atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var materialInput = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);
            if (context.Monitoring.Any(x => (x.Work_Order == materialInput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");
            foreach (var item in materialInput.ConsumptionProducts)
            {
                var product = context.ConsumptionProduct.SingleOrDefault(x => x.Id == item.ProductId);
                product.Stock -= item.Quantity;
                context.ConsumptionProduct.Update(product);
            }
            foreach (var c in materialInput.PermanentProducts)
            {
                var p = context.ConsumptionProduct.Find(c.ProductId);
                p.Stock -= 1;
                context.ConsumptionProduct.Update(p);
            }
            context.MaterialInput.Remove(materialInput);
            await context.SaveChangesAsync();
            return Ok("Devolução removida com sucesso.");
        }
    }
}