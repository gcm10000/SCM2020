using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class Employee
    {
        public string Id { get; set; }
        public string IdUsers { get; set; }

        public Employee() { }
        public Employee(string idUsers)
        {
            this.IdUsers = idUsers;
        }
    }
}
