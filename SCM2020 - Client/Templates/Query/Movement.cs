using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client.Templates.Query
{
    public class Movement
    {
        public class Product
        {
            public int ProductId { get; set; }
            public int Code { get; set; }
            public string Description { get; set; }
            public double Quantity { get; set; } = 0.00d;
            public string Unity { get; set; }
            public string Patrimony { get; set; }
            public string Movement { get; set; }
            public DateTime MoveDate { get; set; }
        }
        private List<Product> Products = null;
        private DateTime InitialDateTime;
        private DateTime FinalDateTime;
        private string Html;
        public QueryByDateDocument(DateTime InitialDateTime, DateTime FinalDateTime, List<Product> Products)
        {
            this.InitialDateTime = InitialDateTime;
            this.FinalDateTime = FinalDateTime;
            this.Products = Products;

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "QueryByDate.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr>" +
                                    $"<td>{product.Code}</td>" +
                                    $"<td>{product.Description}</td>" +
                                    $"<td>{product.Quantity}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.Patrimony}</td>" +
                                    $"<td>{product.Movement}</td>" +
                                    $"<td>{product.MoveDate}</td>" +
                                "</tr>";
            }
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent); //CONTINUAR EDIÇÃO...
            Html = Html.Replace("@WorkOrder", InitialDateTime.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@FinalDate", InitialDateTime.ToString("dd/MM/yyyy"));
            return Html.ToString();
        }
    }
}
