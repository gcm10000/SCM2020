using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<GroupsId> SuperiorId { get; set; }
        public ICollection<GroupsId> SubalternId { get; set; }
        public Grupo() { }

    }
}
