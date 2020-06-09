using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    class InfoUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Register { get; set; }
        public InfoUser() { }
        public InfoUser(string Id, string Name, string Register)
        {
            this.Id = Id;
            this.Name = Name;
            this.Register = Register;
        }
    }
}
