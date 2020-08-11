using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client
{
    public class QueryByDateDocument
    {
        public class Product
        {
            public int ProductId { get; set; }
            public int Code { get; set; }
            public string Description { get; set; }
            public double StockEntry { get; set; } = 0.00d;
            public double StockDevolution { get; set; } = 0.00d;
            public double TotalStock { get => StockEntry + StockDevolution; }
            public double Output { get; set; } = 0.00d;
            public double CurrentBalance { get => TotalStock - Output; }
            public double MinimumStock { get; set; } = 0.00d;
            public double MaximumStock { get; set; } = 0.00d;
            public string Unity { get; set; }
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
                                    $"<td>{product.StockEntry}</td>" +
                                    $"<td>{product.StockDevolution}</td>" +
                                    $"<td>{product.TotalStock}</td>" +
                                    $"<td>{product.Output}</td>" +
                                    $"<td>{product.CurrentBalance}</td>" +
                                    $"<td>{product.MinimumStock}</td>" +
                                    $"<td>{product.MaximumStock}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                "</tr>";
            }
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            Html = Html.Replace("@InitialDate", InitialDateTime.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@FinalDate", InitialDateTime.ToString("dd/MM/yyyy"));
            return Html.ToString();
        }

    }
}
