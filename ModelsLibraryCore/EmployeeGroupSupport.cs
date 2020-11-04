using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class EmployeeGroupSupport
    {
        public int GroupEmployee1Id { get; set; }
        public GroupEmployees GroupEmployeesParent { get; set; }
        public int GroupEmployee2Id { get; set; }
        public GroupEmployees GroupEmployeesChild { get; set; }

    }
}
