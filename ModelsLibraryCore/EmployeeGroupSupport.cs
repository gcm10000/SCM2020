using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class EmployeeGroupSupport
    {
        [JsonIgnore]
        public int GroupEmployee1Id { get; set; }
        [JsonIgnore]
        public GroupEmployees GroupEmployeesParent { get; set; }
        [JsonIgnore]
        public int GroupEmployee2Id { get; set; }
        public GroupEmployees GroupEmployeesChild { get; set; }
        public EmployeeGroupSupport() { }
        public EmployeeGroupSupport(int Parent, int Child) { this.GroupEmployee1Id = Parent; this.GroupEmployee2Id = Child; }

    }
}
