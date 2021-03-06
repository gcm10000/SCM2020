﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
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
        public IActionResult Index()
        {
            //var firstGroupEmployee = ControlDbContext.GroupEmployees.Single(x => x.PositionVertical == 0);
            //GroupEmployees(firstGroupEmployee.GroupEmployeesChild.ToList());

            return Ok("");
        }
        //public List<EmployeeGroupSupport> GroupEmployees(List<EmployeeGroupSupport> groupEmployee)
        //{
        //    for (int i = 0; i < groupEmployee.Count; i++)
        //    {
        //        GroupEmployees(groupEmployee[i].GroupEmployeesChild.GroupEmployeesChild.ToList());
        //    }
        //}


        //[HttpGet("teste")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Index()
        //{
        //    //Receber todo o organograma em JSON
        //    GroupEmployees group1 = new GroupEmployees();
        //    Employee E1 = new Employee();
        //    E1.UsersId = "E1";
        //    E1.BusinessId = 1;
        //    group1.GroupEmployeesChild = new List<EmployeeGroupSupport>();
        //    group1.Employees = new List<Employee>();
        //    group1.Employees.Add(E1);
        //    EmployeeGroupSupport s1 = new EmployeeGroupSupport();
        //    s1.GroupEmployeesParent = group1;

        //    GroupEmployees group2 = new GroupEmployees();
        //    List<Employee> employees = new List<Employee>();
            
        //    Employee E2 = new Employee();
        //    E2.UsersId = "E2";
        //    E2.BusinessId = 2;
        //    employees.Add(E2);

        //    Employee E3 = new Employee();
        //    E3.UsersId = "E3";
        //    E3.BusinessId = 2;
        //    employees.Add(E3);

        //    group2.Employees = employees;
        //    s1.GroupEmployeesChild = group2;

        //    group1.GroupEmployeesChild.Add(s1);

        //    ControlDbContext.GroupEmployees.Add(group1);
        //    await ControlDbContext.SaveChangesAsync();

        //    return Ok(group1.ToJson());
        //}
        [HttpGet("AllEmployees")]
        public IActionResult GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            var groups = ControlDbContext.GroupEmployees.ToList();
            foreach (var group in groups)
            {
                foreach (var employee in group.Employees)
                {
                    employees.Add(employee);
                }
            }
            return Ok(employees);
        }
        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup()
        {
            var raw = await Helper.RawFromBody(this);
            var group = JsonConvert.DeserializeObject<GroupEmployees>(raw);
            ControlDbContext.GroupEmployees.Add(group);
            await ControlDbContext.SaveChangesAsync();
            return Ok("Grupo adicionado com sucesso.");
        }
        //Esse método atualiza os funcionários presentes no grupo. Sobrescreve a lista anterior.
        [HttpPost("FillEmployeeInGroup/{id}")]
        public async Task<IActionResult> FillEmployeeInGroup(int id)
        {
            var raw = await Helper.RawFromBody(this);
            var employeesId = JsonConvert.DeserializeObject<List<int>>(raw);

            var group = ControlDbContext.GroupEmployees.Find(id);
            group.Employees = new List<Employee>();
            foreach (var employeeId in employeesId)
            {
                var employee = ControlDbContext.Employees.Find(employeeId);

                group.Employees.Add(employee);
            }

            ControlDbContext.GroupEmployees.Update(group);
            await ControlDbContext.SaveChangesAsync();
            return Ok("Grupo preenchido com sucesso.");
        }
        [HttpPost("AddNode")]
        public async Task<IActionResult> AddNode()
        {
            //Dois grupos por vez compõem um nó
            var raw = await Helper.RawFromBody(this);
            var node = JsonConvert.DeserializeObject<NewNode>(raw);

            var parent = ControlDbContext.GroupEmployees.Find(node.GroupEmployeesParent);
            var child = ControlDbContext.GroupEmployees.Find(node.GroupEmployeesParent);

            parent.GroupEmployeesChild.Add(new EmployeeGroupSupport(Parent: parent.Id, Child: child.Id));
            child.GroupEmployeesParent.Add(new EmployeeGroupSupport(Parent: parent.Id, Child: child.Id));
            
            ControlDbContext.GroupEmployees.Update(parent);
            ControlDbContext.GroupEmployees.Update(child);
            await ControlDbContext.SaveChangesAsync();
            return Ok("Nó adicionado com sucesso.");
        }
    }
}
