using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsLibraryCore
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UsersId { get; set; }
        public int? BusinessId { get; set; }
        public GroupEmployees Employees { get; set; }
        public Employee() { }
        public Employee(string idUsers)
        {
            this.UsersId = idUsers;
        }
    }
}
