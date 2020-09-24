using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client
{
    public class InventoryOfficerPreview
    {
        public class Product
        {
            public int ID { get; }
            public int SKU { get; }
            public string Description { get; }
            public double Quantity { get; }
            public string Unity { get; }

            public Product(int ID, int SKU, string Description, double Quantity, string Unity)
            {
                this.ID = ID;
                this.SKU = SKU;
                this.Description = Description;
                this.Quantity = Quantity;
                this.Unity = Unity;
            }
        }

        private string html = string.Empty;
        private ICollection<Product> Products;
        public InventoryOfficerPreview(ICollection<Product> Products)
        {
            this.Products = Products;
            var pathFileHtml = Path.Combine(Helper.CurrentDirectory, "templates", "InventoryOfficer.html");
            html = System.IO.File.ReadAllText(pathFileHtml);
        }

        public string RenderizeHTML() 
        {
            StringBuilder StringBuilder = new StringBuilder();
            foreach (var product in Products)
            {
                string line =
                    "<tr>" +
                    $"  <th>{product.SKU}</th>" +
                    $"  <td>{product.Description}</td>" +
                    $"  <td>{product.Quantity}</td>" +
                    $"  <td>{product.Unity}</td>" +
                    "</tr>";
                StringBuilder.AppendLine(line);
            }
            html = html.Replace("@PRODUCTS", StringBuilder.ToString());
            html = html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);
            return html;
        }
    }
}
