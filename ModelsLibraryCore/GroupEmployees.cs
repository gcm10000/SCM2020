using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsLibraryCore
{
    public class GroupEmployees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
        [JsonIgnore]
        public ICollection<EmployeeGroupSupport> GroupEmployeesParent { get; set; }
        public ICollection<EmployeeGroupSupport> GroupEmployeesChild { get; set; }
        public GroupEmployees() { }
        //public GroupEmployees(CompanyPosition position) { this.Position = position; }
    }
}
