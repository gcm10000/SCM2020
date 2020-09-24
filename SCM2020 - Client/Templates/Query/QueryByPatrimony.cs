using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client.Templates.Query
{
    public class QueryByPatrimony
    {
        private string Html;

        List<SCM2020___Client.Models.QueryByPatrimony> Products;
        public QueryByPatrimony(List<SCM2020___Client.Models.QueryByPatrimony> Products)
        {
            this.Products = Products;
            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "QueryByPatrimony.html");
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
                                    $"<td>{product.Patrimony}</td>" +
                                    $"<td>{product.WorkOrder}</td>" +
                                    $"<td>{product.Employee}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            Html = Html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);

            return Html.ToString();
        }
    }
}
