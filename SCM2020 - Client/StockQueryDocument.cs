using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client
{
    public class StockQueryDocument
    {
        private List<Models.StockQuery> Products;
        private string Html;
        public StockQueryDocument(List<Models.StockQuery> Products)
        {

            this.Products = Products;

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "StockQuery.html");
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
                                    $"<td>{product.MininumStock}</td>" +
                                    $"<td>{product.Stock}</td>" +
                                    $"<td>{product.MaximumStock}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.Localization}</td>" +
                                    $"<td>{product.Group}</td>" +
                                "</tr>";
            }
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            Html = Html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);
            return Html.ToString();
        }

    } 
}
