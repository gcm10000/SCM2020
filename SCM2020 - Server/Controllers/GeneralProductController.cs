﻿using Microsoft.AspNetCore.Authorization;
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
using System;

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
            await context.Entry(newProduct).GetDatabaseValuesAsync();
            return Ok(new Result(newProduct.Id, "Produto adicionado com sucesso.", null));
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
            var lProduct = context.ConsumptionProduct.Include(x => x.Photos).ToList();
            return Ok(lProduct);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ShowById(int id)
        {
            var product = await context.ConsumptionProduct.Include(x => x.Photos).SingleOrDefaultAsync(x => x.Id == id);
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
            var product = context.ConsumptionProduct.Include(x => x.Photos).SingleOrDefault(x => x.Code == code);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"O registro com o código {code} não existe.");
            }
        }
        [HttpGet("ReverseSearch/{code}")]
        public IActionResult ReverseSearch(int code)
        {
            //A partir do código produto, exibir todas as ordens de serviço e notas fiscais
            var outputs = context.MaterialOutput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            var inputs = context.MaterialInputByVendor.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            var devolutions = context.MaterialInput.Include(x => x.ConsumptionProducts).Include(x => x.PermanentProducts).ToList();
            var consumptionProducts = context.ConsumptionProduct.Include(x => x.Photos).ToList();
            var permanentProducts = context.PermanentProduct.ToList();
            var monitorings = context.Monitoring.ToList();

            List<ReverseSearch> result = new List<ReverseSearch>();
            //Adicionar na lista se a ordem de serviço não existir e se a movimentação for diferente
            foreach (var output in outputs)
            {
                foreach (var auxiliarConsumption in output.ConsumptionProducts)
                {
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == auxiliarConsumption.ProductId);
                    if (consumptionProduct.Code == code)
                    {
                        if (!(result.Any(x => (x.Movement == Movement.Output) && (x.WorkOrder == output.WorkOrder))))
                        {
                            var monitoring = monitorings.Single(x => x.Work_Order == output.WorkOrder);
                            result.Add(new ModelsLibraryCore.ReverseSearch()
                            {
                                WorkOrder = output.WorkOrder,
                                Stock = auxiliarConsumption.Quantity,
                                Unity = consumptionProduct.Unity,
                                Movement = Movement.Output,
                                ServiceLocalization = monitoring.ServiceLocation,
                                MovingDate = monitoring.MovingDate,
                                ClosingDate = monitoring.ClosingDate,
                                Invoice = null,
                                Type = ModelsLibraryCore.Type.Consumpter,
                                Patrimony = null
                            });
                        }
                        else
                        {
                            var y = result.Single(x => (x.Movement == Movement.Output) && (x.WorkOrder == output.WorkOrder));
                            y.Stock += auxiliarConsumption.Quantity;
                        }
                    }
                }
                foreach (var auxiliarPermanent in output.PermanentProducts)
                {
                    var permanentProduct = permanentProducts.Single(x => x.Id == auxiliarPermanent.ProductId);
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == permanentProduct.InformationProduct);

                    if (consumptionProduct.Code == code)
                    {
                        var monitoring = monitorings.Single(x => x.Work_Order == output.WorkOrder);
                        result.Add(new ModelsLibraryCore.ReverseSearch()
                        {
                            WorkOrder = output.WorkOrder,
                            Stock = 1,
                            Unity = consumptionProduct.Unity,
                            Type = ModelsLibraryCore.Type.Permanent,
                            Movement = Movement.Output,
                            MovingDate = monitoring.MovingDate,
                            ClosingDate = monitoring.ClosingDate,
                            ServiceLocalization = monitoring.ServiceLocation,
                            Patrimony = permanentProduct.Patrimony,
                            Invoice = null
                        });
                    }
                }
            }

            foreach (var input in inputs)
            {
                foreach (var auxiliarConsumption in input.ConsumptionProducts)
                {
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == auxiliarConsumption.ProductId);
                    if (consumptionProduct.Code == code)
                    {
                        if (!(result.Any(x => (x.Movement == Movement.Input) && (x.Invoice == input.Invoice))))
                        {
                            result.Add(new ModelsLibraryCore.ReverseSearch()
                            {
                                WorkOrder = null,
                                Stock = auxiliarConsumption.Quantity,
                                Unity = consumptionProduct.Unity,
                                Movement = Movement.Input,
                                ServiceLocalization = string.Empty,
                                MovingDate = input.MovingDate,
                                ClosingDate = null,
                                Invoice = input.Invoice, 
                                Patrimony = null, 
                                Type = ModelsLibraryCore.Type.Consumpter
                            });
                        }
                        else
                        {
                            var y = result.Single(x => (x.Movement == Movement.Input) && (x.Invoice == input.Invoice));
                            y.Stock += auxiliarConsumption.Quantity;
                        }
                    }
                }
                foreach (var auxiliarPermanent in input.PermanentProducts)
                {
                    var permanentProduct = permanentProducts.Single(x => x.Id == auxiliarPermanent.ProductId);
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == permanentProduct.InformationProduct);

                    if (consumptionProduct.Code == code)
                    {
                        result.Add(new ModelsLibraryCore.ReverseSearch()
                        {
                            WorkOrder = null,
                            Stock = 1,
                            Unity = consumptionProduct.Unity,
                            Type = ModelsLibraryCore.Type.Permanent,
                            Movement = Movement.Input,
                            MovingDate = input.MovingDate,
                            ClosingDate = null,
                            ServiceLocalization = null,
                            Patrimony = permanentProduct.Patrimony,
                            Invoice = input.Invoice
                        });
                    }
                }
            }

            foreach (var devolution in devolutions)
            {
                foreach (var auxiliarConsumption in devolution.ConsumptionProducts)
                {
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == auxiliarConsumption.ProductId);
                    if (consumptionProduct.Code == code)
                    {
                        if (!(result.Any(x => (x.Movement == Movement.Devolution) && (x.WorkOrder == devolution.WorkOrder))))
                        {
                            var monitoring = monitorings.Single(x => x.Work_Order == devolution.WorkOrder);
                            result.Add(new ModelsLibraryCore.ReverseSearch()
                            {
                                WorkOrder = devolution.WorkOrder,
                                Stock = auxiliarConsumption.Quantity,
                                Unity = consumptionProduct.Unity,
                                Movement = Movement.Devolution,
                                ServiceLocalization = monitoring.ServiceLocation,
                                MovingDate = monitoring.MovingDate,
                                ClosingDate = monitoring.ClosingDate,
                                Invoice = null, 
                                Type = ModelsLibraryCore.Type.Consumpter,
                                Patrimony = null
                            });
                        }
                        else
                        {
                            var y = result.Single(x => (x.Movement == Movement.Devolution) && (x.WorkOrder == devolution.WorkOrder));
                            y.Stock += auxiliarConsumption.Quantity;
                        }
                    }
                }
                foreach (var auxiliarPermanent in devolution.PermanentProducts)
                {
                    var permanentProduct = permanentProducts.Single(x => x.Id == auxiliarPermanent.ProductId);
                    var consumptionProduct = consumptionProducts.Single(x => x.Id == permanentProduct.InformationProduct);

                    if (consumptionProduct.Code == code)
                    {
                        var monitoring = monitorings.Single(x => x.Work_Order == devolution.WorkOrder);
                        result.Add(new ModelsLibraryCore.ReverseSearch()
                        {
                            WorkOrder = devolution.WorkOrder,
                            Stock = 1,
                            Unity = consumptionProduct.Unity,
                            Type = ModelsLibraryCore.Type.Permanent,
                            Movement = Movement.Devolution,
                            MovingDate = monitoring.MovingDate,
                            ClosingDate = monitoring.ClosingDate,
                            ServiceLocalization = monitoring.ServiceLocation,
                            Patrimony = permanentProduct.Patrimony,
                            Invoice = null
                        });
                    }
                }
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return Ok(result);
        }
        [HttpGet("CheckCode/{code}")]
        public IActionResult CheckCode(int code)
        {
            var result = context.ConsumptionProduct.Any(x => x.Code == code);
            return Ok(result);
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
                lproduct = context.ConsumptionProduct.Include(x => x.Photos).ToList()
                    .Where(x => string.Join(" ", x.Description, x.Code).MultiplesContainsWords(querySplited));
            }
            else
            {
                lproduct = context.ConsumptionProduct.Include(x => x.Photos).ToList()
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
                string relativeUrl = Helper.Combine("img", imageInput.ProductId.ToString() + "-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + Path.GetExtension(imageInput.Image.FileName));
                var product = context.ConsumptionProduct.Include(x => x.Photos).Single(x => x.Id == imageInput.ProductId);
                if (product.Photos == null)
                {
                    product.Photos = new List<Photo>();
                }
                product.Photos.Add(new Photo() { Path = relativeUrl });
                string fullName = Path.Combine(_env.WebRootPath, relativeUrl);

                using (var stream = System.IO.File.Create(fullName))
                {
                    using (var ms = new MemoryStream())
                    {
                        await imageInput.Image.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        //Averigua se é uma imagem válida
                        if (!((Helper.GetImageFormat(fileBytes) == ImageFormat.tiff) || (Helper.GetImageFormat(fileBytes) == ImageFormat.unknown)))
                        {
                            await imageInput.Image.CopyToAsync(stream);
                        }
                        else
                        {
                            return BadRequest(new Result(imageInput.ProductId, "Este arquivo não é uma imagem ou não é um formato compatível.", null));
                        }
                    }

                }
                await context.SaveChangesAsync();
                return Ok(new Result(imageInput.ProductId, "Imagem enviada com sucesso.", relativeUrl));
            }
            else
            {
                return BadRequest(new Result(imageInput.ProductId, "Imagem maior ou igual a 10 MB. Envie um tamanho menor.", null));
            }
        }
        [HttpDelete("RemoveImage/{ProductId}/{PhotoId}")]
        public async Task<IActionResult> RemoveImage(int ProductId, int PhotoId)
        {
            var product = context.ConsumptionProduct.Include(x => x.Photos).Single(x => x.Id == ProductId);
            var photo = product.Photos.Single(x => x.Id == PhotoId);
            string fullName = Path.Combine(_env.WebRootPath, photo.Path);
            System.IO.File.Delete(fullName);
            product.Photos.Remove(photo);
            await context.SaveChangesAsync();
            return Ok(new Result(ProductId, "Imagem removida com sucesso.", null));
        }
    }
}
