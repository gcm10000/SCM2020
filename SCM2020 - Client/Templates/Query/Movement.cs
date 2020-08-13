using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly string workOrder;
        private readonly int registerApplication;
        private readonly string application;
        private readonly string sector;
        private readonly string situation;
        private readonly string serviceLocalization;
        private readonly DateTime workOrderDate;
        private readonly DateTime? closureWorkOrder;

        private List<Product> Products = null;
        private string Html;
        public Movement(string WorkOrder, int RegisterApplication, string Application, string Sector, string Situation, string ServiceLocalization, DateTime WorkOrderDate, DateTime? ClosureWorkOrder, List<Product> Products)
        {
            workOrder = WorkOrder;
            registerApplication = RegisterApplication;
            application = Application;
            sector = Sector;
            situation = Situation;
            serviceLocalization = ServiceLocalization;
            workOrderDate = WorkOrderDate;
            closureWorkOrder = ClosureWorkOrder;
            this.Products = Products;

            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "QueryByDate.html");
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
            Html = Html.Replace("@WorkOrder", workOrder); 
            Html = Html.Replace("@RegisterApplication", registerApplication.ToString());
            Html = Html.Replace("@Application", application);
            Html = Html.Replace("@Sector", sector);
            Html = Html.Replace("@Situation", situation);
            Html = Html.Replace("@ServiceLocalization", serviceLocalization);
            Html = Html.Replace("@WorkOrderDate", workOrderDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@FinalDate", (closureWorkOrder == null) ? "" : closureWorkOrder.Value.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@LISTOFPRODUCTS", itemsContent);
            return Html.ToString();
        }
    }
}
