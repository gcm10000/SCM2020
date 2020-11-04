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
        public CompanyPosition Position { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<EmployeeGroupSupport> GroupEmployees1 { get; set; }
        public ICollection<EmployeeGroupSupport> GroupEmployees2 { get; set; }
        public GroupEmployees() { }
        public GroupEmployees(CompanyPosition position) { this.Position = position; }
    }
}
