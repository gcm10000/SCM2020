using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        public IActionResult Index()
        {
            return Ok("CONTROLE DE TESTE 123");
        }
    }
}
