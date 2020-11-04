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
            GroupEmployees group1 = new GroupEmployees(CompanyPosition.Director);
            Employee E1 = new Employee();
            E1.UsersId = "E1";
            E1.BusinessId = 1;
            group1.GroupEmployees1 = new List<EmployeeGroupSupport>();
            group1.Employees = new List<Employee>();
            group1.Employees.Add(E1);
            EmployeeGroupSupport s1 = new EmployeeGroupSupport();
            s1.GroupEmployeesParent = group1;
            
            GroupEmployees group2 = new GroupEmployees(CompanyPosition.Engineer);
            Employee E2 = new Employee();
            E2.UsersId = "E2";
            E2.BusinessId = 2;
            
            s1.GroupEmployeesChild = group2;

            group1.GroupEmployees1.Add(s1);
            
            return Ok(group1.ToJson());
        }
        public void GetAllEmployees(ICollection<Employee> employees)
        {

        }
    }
}
