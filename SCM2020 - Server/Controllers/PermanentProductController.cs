﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelsLibraryCore;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PermanentProductController : ControllerBase
    {
        ControlDbContext context;
        public PermanentProductController(ControlDbContext context)
        {
            this.context = context;
        }
        //[Authorize(Roles = Roles.Administrator)]
        [HttpPost("Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var raw = await Helper.RawFromBody(this);
            PermanentProduct product = new PermanentProduct(raw);
            context.PermanentProduct.Add(product);
            await context.SaveChangesAsync();
            return Ok("Migração feita com sucesso.");
        }
        [HttpPost("RegisterInOutput/{patrimony}")]
        public async Task<IActionResult> RegisterInOutput(string patrimony)
        {
            var product = context.PermanentProduct.Single(x => x.Patrimony == patrimony);
            var output = context.MaterialOutput.Include(x => x.PermanentProducts).SingleOrDefault(x => x.WorkOrder == product.WorkOrder);
            //É PRECISO CONVERTER UM PRODUTO CONSUMÍVEL EM PERMAMENTE
            //PARA ISSO É PRECISO APAGAR UM PRODUTO CONSUMÍVEL DENTRO DA MOVIMENTAÇÃO DE CÓDIGO IGUAL
            //E ADICIONAR UM PRODUTO PERMAMENTE NA COLEÇÃO DE PERMANENTES
            output.PermanentProducts.Add(new AuxiliarPermanent() { Date = DateTime.Now, ProductId = product.Id, SCMEmployeeId = null });
            await context.SaveChangesAsync();
            return Ok("Migração feita com sucesso.");

        }
        //Add new product content every information about
        [HttpPost("Add")]
        public async Task<IActionResult> Add()
        {
            var raw = await Helper.RawFromBody(this);
            PermanentProduct newProduct = new PermanentProduct(raw);
            if (!context.ConsumptionProduct.Any(x => x.Id == newProduct.InformationProduct))
                return BadRequest("Não existe informação de produto com este id.");

            var infoProduct = context.ConsumptionProduct.Find(newProduct.InformationProduct);
            infoProduct.Stock += 1;

            context.PermanentProduct.Add(newProduct);
            await context.SaveChangesAsync();
            return Ok("Produto adicionado com sucesso.");
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var permanentProduct = JsonConvert.DeserializeObject<PermanentProduct>(raw);
            permanentProduct.Id = id;

            context.PermanentProduct.Update(permanentProduct);
            await context.SaveChangesAsync();
            return Ok("Atualizado com sucesso.");
        }
        [HttpGet]
        public IActionResult ShowAll()
        {
            return Ok(context.PermanentProduct.ToList());
        }
        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            var product = context.PermanentProduct.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"O registro com o id {id} não existe.");
            }
        }
        [HttpGet("Search/{patrimony}")]
        public IActionResult SearchByPatrimony(string patrimony)
        {
            patrimony = System.Uri.UnescapeDataString(patrimony);

            if (string.IsNullOrWhiteSpace(patrimony))
                return Ok();

            patrimony = patrimony.Trim();

            var productPermanent = context.PermanentProduct.Where(x => x.Patrimony.Contains(patrimony));
            if (productPermanent != null)
            {
                return Ok(productPermanent);
            }
            else
            {
                return BadRequest($"O registro com o patrimônio {patrimony} não existe.");
            }
        }

        //Remove by ID
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            PermanentProduct product = context.PermanentProduct.Find(id);
            context.PermanentProduct.Remove(product);

            var infoProduct = context.ConsumptionProduct.Find(product.InformationProduct);
            infoProduct.Stock -= 1;

            await context.SaveChangesAsync();
            return Ok("Produto removido com sucesso.");
        }
    }
}
