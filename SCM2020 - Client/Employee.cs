using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Employee() { }
        public Employee(string name)
        {
            this.Name = name;
        }
    }
}
