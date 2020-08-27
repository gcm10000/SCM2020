using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client.Models
{
    public class MaterialMovementPrintExport
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public string Unity { get; set; }
        public string Patrimony { get; set; }
        public string Movement { get; set; }
        public DateTime MoveDate { get; set; }
    }
}
