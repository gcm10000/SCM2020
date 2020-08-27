using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client.Templates.Movement
{
    public class MaterialOutput
    {
        private string Html;

        List<Models.MaterialMovementPrintExport> Products;
        public MaterialOutput(List<Models.MaterialMovementPrintExport> Products)
        {
            this.Products = Products;
            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "MaterialOutput.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr>" +
                                    $"<td>{product.MoveDate.ToString("dd/MM/yyyy")}</td>" +
                                    $"<td>{product.Code}</td>" +
                                    $"<td>{product.Description}</td>" +
                                    $"<td>{product.Quantity}</td>" +
                                    $"<td>{product.Unity}</td>" +
                                    $"<td>{product.Movement}</td>" +
                                    $"<td>{product.Patrimony}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@WorkOrderDate", itemsContent);
            Html = Html.Replace("@WorkOrder", itemsContent);
            Html = Html.Replace("@Sitution", itemsContent);
            Html = Html.Replace("@RegisterApplicant", itemsContent);
            Html = Html.Replace("@NameApplicant", itemsContent);
            Html = Html.Replace("@Sector", itemsContent);
            Html = Html.Replace("@LISTOFITEMS", itemsContent);
            return Html.ToString();
        }
    }
}
