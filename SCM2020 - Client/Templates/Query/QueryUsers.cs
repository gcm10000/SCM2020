using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCM2020___Client.Templates.Query
{
    public class QueryUsers
    {
        List<Models.QueryUsers> queryUsers;
        string Html;
        public QueryUsers(List<Models.QueryUsers> queryUsers)
        {
            this.queryUsers = queryUsers;
            var pathFileHtml = Path.Combine(Directory.GetCurrentDirectory(), "templates", "query", "QueryUsers.html");
            Html = System.IO.File.ReadAllText(pathFileHtml);
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var user in queryUsers)
            {
                itemsContent += "<tr>" +
                                    $"<td>{user.Name}</td>" +
                                    $"<td>{user.Register}</td>" +
                                    $"<td>{user.ThirdParty}</td>" +
                                    $"<td>{user.Sector}</td>" +
                                "</tr>";
            }

            Html = Html.Replace("@LISTOFUSERS", itemsContent);
            Html = Html.Replace("@BootstrapDirectory", new System.Uri(Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);

            return Html;
        }
    }
}
