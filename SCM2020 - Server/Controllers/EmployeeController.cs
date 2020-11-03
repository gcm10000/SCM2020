using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelsLibraryCore;
using SCM2020___Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EmployeeController : ControllerBase
    {
        ControlDbContext ControlDbContext;
        UserManager<ApplicationUser> userManager;

        public EmployeeController(ControlDbContext controlDbContext, UserManager<ApplicationUser> userManager)
        {
            this.ControlDbContext = controlDbContext;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get()
        {
            //Receber todo o organograma em JSON
            List<Employee> employees = new List<Employee>()
            {
                new Employee(){ Id = 1, Position = CompanyPosition.Director, BusinessId = 1, SuperiorId = null, UsersId = "aaa" },
                new Employee(){ Id = 2, Position = CompanyPosition.Engineer, BusinessId = 2, SuperiorId = 1, UsersId = "aab" },
                new Employee(){ Id = 3, Position = CompanyPosition.Engineer, BusinessId = 2, SuperiorId = 1, UsersId = "aac" },
                new Employee(){ Id = 4, Position = CompanyPosition.Manager, BusinessId = 2, SuperiorId = 1, UsersId = "aad" },
                new Employee(){ Id = 5, Position = CompanyPosition.Manager, BusinessId = 2, SuperiorId = 1, UsersId = "aae" },
            };
            List<GroupEmployees> groupEmployees = new List<GroupEmployees>()
            {
                new GroupEmployees(){ Id = 1, SuperiorId = 1, EmployeeId = 1 },
                new GroupEmployees(){ Id = 2, SuperiorId = 2, EmployeeId = 2 },
                new GroupEmployees(){ Id = 3, SuperiorId = 2, EmployeeId = 3 },
                new GroupEmployees(){ Id = 4, SuperiorId = 3, EmployeeId = 5 },
            };

            return Ok();
        }
        public void GetAllEmployees(ICollection<Employee> employees)
        {

        }
    }
}
