using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using ModelsLibraryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.SCM)]
    public class OutputController : ControllerBase
    {
        ControlDbContext context;
        public OutputController(ControlDbContext context) { this.context = context; }
        [HttpGet]
        public IActionResult ShowAll()
        {
            var lOutput = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            return Ok(lOutput);
        }
        [HttpGet("{id}")]
        public IActionResult ShowById(int id)
        {
            var output = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);
            return Ok(output);
        }
        [HttpGet("WorkOrder/{workorder}")]
        public IActionResult ShowByWorkOrder(string workorder)
        {
            var output = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.WorkOrder == workorder);
            return Ok(output);
        }
        [HttpGet("Date/{StartDay}-{StartMonth}-{StartYear}/{EndDay}-{EndMonth}-{EndYear}")]
        public IActionResult ShowByDate(int StartDay, int StartMonth, int StartYear, int EndDay, int EndMonth, int EndYear)
        {
            DateTime dateStart = new DateTime(StartYear, StartMonth, StartDay);
            DateTime dateEnd = new DateTime(EndYear, EndMonth, EndDay);
            List<AuxiliarConsumption> outputs = new List<AuxiliarConsumption>();
            foreach (var output in context.MaterialOutput.ToList())
            {
                outputs.AddRange(output.ConsumptionProducts.Where(t => (t.Date >= dateStart) && (t.Date <= dateEnd)));
            }

            return Ok(outputs.ToList());
        }
        [HttpPost("Add")]
        public async Task<IActionResult> NewOutput()
        {
            var b = Helper.GetToken(out System.IdentityModel.Tokens.Jwt.JwtSecurityToken token, this);
            if (!b)
                return BadRequest("Por favor, faça login.");
            var id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var raw = await Helper.RawFromBody(this);
            var output = new MaterialOutput(raw, id);
            var monitoring = context.Monitoring.SingleOrDefault(x => x.Work_Order == output.WorkOrder);
            if (monitoring == null)
                return BadRequest("Ordem de serviço inexistente.");
            if (monitoring.Situation == true)
                return BadRequest("Ordem de serviço fechada.");
            var arrayConsumption = output.ConsumptionProducts.ToList();
            var arrayPermanent = output.PermanentProducts.ToList();

            //Check if every objects of arrayConsumption (less) are inside list ConsumptionProduct (bigger)
            bool MatchesConsumption = arrayConsumption
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.ProductId));
            bool MatchesPermanent = arrayPermanent
                .All(x => context.ConsumptionProduct
                .Any(y => y.Id == x.ProductId));
            if (!MatchesConsumption)
                return BadRequest("Há produtos de consumo não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");
            if (!MatchesPermanent)
                return BadRequest("Há produtos permanentes não cadastrados sendo solicitado na movimentação de saída. Verifique e tente novamente.");

            context.MaterialOutput.Add(output);

            //arrayConsumption.ForEach(x => context.ConsumptionProduct.Find(x.ProductId).Stock -= 1);
            //arrayPermanent.ForEach(x => context.ConsumptionProduct.Find(x.ProductId).Stock -= 1);

            foreach (var p in arrayConsumption)
            {
                var c = context.ConsumptionProduct.Find(p.ProductId);
                c.Stock -= p.Quantity;
                context.ConsumptionProduct.Update(c);
            }
            foreach (var p in arrayPermanent)
            {
                var c = context.ConsumptionProduct.Find(p.ProductId);
                c.Stock -= 1;
                context.ConsumptionProduct.Update(c);
            }
            await context.SaveChangesAsync();
            return Ok("Saída feita com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var materialOutput = JsonConvert.DeserializeObject<MaterialOutput>(raw);
            var output = context.MaterialOutput.Include(x => x.PermanentProducts).Include(x => x.ConsumptionProducts).SingleOrDefault(x => x.Id == id);
            output.MovingDate = materialOutput.MovingDate;
            output.EmployeeRegistration = materialOutput.EmployeeRegistration;
            output.RequestingSector = materialOutput.RequestingSector;
            output.ServiceLocation = materialOutput.ServiceLocation;
            output.WorkOrder = materialOutput.WorkOrder;
            
            var lConsumpter = new List<AuxiliarConsumption>();
            lConsumpter.AddRange(output.ConsumptionProducts);
            var lPermanent = new List<AuxiliarPermanent>();
            lPermanent.AddRange(output.PermanentProducts);
            output.ConsumptionProducts = materialOutput.ConsumptionProducts;
            output.PermanentProducts = materialOutput.PermanentProducts;

            if (context.Monitoring.Any(x => (x.Work_Order == materialOutput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");
            List<int> ConsumpterProductIds = new List<int>();
            List<int> PermanentsProductIds = new List<int>();
            foreach (var p in output.ConsumptionProducts)
            {
                if (!ConsumpterProductIds.Contains(p.ProductId))
                    ConsumpterProductIds.Add(p.ProductId);
            }
            foreach (var p in output.PermanentProducts)
            {
                if (!PermanentsProductIds.Contains(p.ProductId))
                    PermanentsProductIds.Add(p.ProductId);
            }

            //bool allEquals = output.ConsumptionProducts.All(x => lConsumpter
            //.Any(y => (x.Date == y.Date) && (x.ProductId == y.ProductId) && (x.Quantity == y.Quantity)));

            foreach (var currentId in ConsumpterProductIds)
            {
                var products = lConsumpter.Where(x => x.ProductId == currentId);
                double quantityProduct = 0d;
                foreach (var p in products)
                {
                    quantityProduct += p.Quantity;
                }
                double quantityNewProduct = 0d;
                var newProducts = output.ConsumptionProducts.ToList().Where(x => x.ProductId == currentId);
                foreach (var p in newProducts)
                {
                    quantityNewProduct += p.Quantity;
                }
                var productModify = context.ConsumptionProduct.Find(currentId);
                productModify.Stock += (quantityProduct - quantityNewProduct);
                context.ConsumptionProduct.Update(productModify);
            }

            //permanent
            foreach (var currentId in PermanentsProductIds)
            {
                //oldder - newest
                var productModify = await context.ConsumptionProduct.FindAsync(currentId);
                double oldder = lPermanent.Count(x => x.ProductId == currentId);
                double newest = output.PermanentProducts.Count(x => x.ProductId == currentId);
                if (oldder != newest)
                {
                    productModify.Stock += oldder - newest;
                    context.ConsumptionProduct.Update(productModify);
                }
            }
            context.MaterialOutput.Update(materialOutput);
            await context.SaveChangesAsync();
            return Ok("Movimentação de saída atualizada com sucesso.");
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var materialOutput = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).SingleOrDefault(x => x.Id == id);

            if (context.Monitoring.Any(x => (x.Work_Order == materialOutput.WorkOrder) && (x.Situation == true)))
                return BadRequest("Ordem de serviço fechada.");
            //Products reallocation
            foreach (var c in materialOutput.ConsumptionProducts)
            {
                var p = context.ConsumptionProduct.Find(c.ProductId);
                p.Stock += c.Quantity;
                context.ConsumptionProduct.Update(p);
            }
            foreach (var c in materialOutput.PermanentProducts)
            {
                var p = context.ConsumptionProduct.Find(c.ProductId);
                p.Stock += 1;
                context.ConsumptionProduct.Update(p);
            }

            context.MaterialOutput.Remove(materialOutput);
            await context.SaveChangesAsync();
            return Ok("Movimentação de saída removida com sucesso.");
        }
    }
}
