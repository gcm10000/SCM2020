using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupsId> SuperiorIds { get; set; }
        public ICollection<GroupsId> SubalternIds { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public Grupo() { }

    }
}
