//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using SCM2020___Server.Context;
//using SCM2020___Server.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace SCM2020___Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(AuthenticationSchemes = "Bearer")]
//    public class TestController : ControllerBase
//    {
//        //"DefaultConnection": "server=DESKTOP-H7OJP9T\\SQLEXPRESS; database=SCM;Trusted_Connection=True;"
//            public TestController() { }
//        [HttpGet]
//        public IActionResult Get()
//        {
//            var x = MyTest();
//            return Ok(x);
//        }
//        //private string MyTest()
//        //{
//        //    //Primeiro faz o cadastro do produto.
//        //    //Existem dois tipos de produtos: Descartáveis e retornáveis
//        //    //Descartáveis não têm número de patrimônio
//        //    //Retornáveis têm número de patrimônio
//        //    var product = new SpecificProduct();
//        //    product.DateAdd = DateTime.Now;
//        //    product.Status = Status.New;
//        //    product.Patrimony = "21";
//        //    product.InformationProduct = new AboutProduct()
//        //    {
//        //        Code = 1242,
//        //        Description = "Capacitor Eletrolítico-10/mfx50v",
//        //        Unity = "UN",
//        //        Group = new Group() { GroupName = "Geral" },
//        //        Block = "J-D",
//        //        Localization = Localization.Drawer,
//        //        Drawer = 2,
//        //        Vendor = new Vendor() { Name = "BERKANA", Telephone = "00" }
//        //    };
//        //    return JsonConvert.SerializeObject(product, Formatting.Indented);
//        //}
//    }
//}
