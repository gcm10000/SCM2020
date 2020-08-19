using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client.Templates.Query
{
    public class QueryWorkOrderByDate
    {
        private string Html;
        private List<SCM2020___Client.Frames.Query.QueryWorkOrderByDate.QueryWorkOrderByDateDataGrid> WorkOrders;
        private DateTime InitialDate { get; }
        private DateTime FinalDate { get; }
        public QueryWorkOrderByDate(List<SCM2020___Client.Frames.Query.QueryWorkOrderByDate.QueryWorkOrderByDateDataGrid> queryWorkOrderByDate, DateTime InitialDate, DateTime FinalDate)
        {
            this.WorkOrders = queryWorkOrderByDate;
            this.InitialDate = InitialDate;
            this.FinalDate = FinalDate;
            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "QueryWorkOrderByDate.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }

        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var workOrder in WorkOrders)
            {
                itemsContent += "<tr>" +
                                    $"<td>{workOrder.WorkOrder}</td>" +
                                    $"<td>{workOrder.Date.ToString("dd/MM/yyyy")}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@InitialDate", InitialDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@FinalDate", FinalDate.ToString("dd/MM/yyyy"));
            Html = Html.Replace("@LISTOFWORKORDERS", itemsContent);
            return Html.ToString();
        }
    }
}
