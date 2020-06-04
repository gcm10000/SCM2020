using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    class ConsumpterProductDataGrid
    {
        //public string Image { get; set; }
        public int Id { get; set; }
        public int Code { get; set; }
        public double QuantityFuture { get => Quantity + QuantityAdded; }
        public double QuantityAdded { get; set; }
        public double Quantity { get; set; }
        public string Description { get; set; }
    }
    class PermanentProductDataGrid : ConsumpterProductDataGrid
    {
        public string Patrimony { get; set; }
    }
}
