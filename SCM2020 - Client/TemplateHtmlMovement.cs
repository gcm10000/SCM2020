using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SCM2020___Client
{
    /// <summary>
    /// Template reproduzido em HTML para visualização das movimentações com ordem de serviço.
    /// </summary>
    public class DocumentMovement
    {
        public class QueryMovement
        {
            public int RegisterApplication { get; set; }
            public string SolicitationEmployee { get; set; }
            public string WorkOrder { get; set; }
            public string Situation { get; set; }
            public string Sector { get; set; }
            public string ServiceLocalizationTextBox { get; set; }
            public DateTime WorkOrderDate { get; set; }
        }
        public class Product
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public double Quantity { get; set; }
            public string Unity { get; set; }
            public string Patrimony { get; set; }
            public string Movement { get; set; }
            public DateTime MoveDate { get; set; }
        }
        public class ResultSearch
        {
            public List<DocumentMovement.Product> ProductsToShow { get; }
            public Monitoring Monitoring { get; }
            public InfoUser InfoUser { get; }
            public QueryMovement InformationQuery { get; }
            public ResultSearch(List<DocumentMovement.Product> ProductsToShow, Monitoring Monitoring, InfoUser InfoUser, QueryMovement InformationQuery)
            {
                this.ProductsToShow = ProductsToShow;
                this.Monitoring = Monitoring;
                this.InfoUser = InfoUser;
                this.InformationQuery = InformationQuery;
            }
        }

        /// <summary>
        /// Conjunto de produtos.
        /// </summary>
        public List<DocumentMovement.Product> Products { get; }
        /// <summary>
        /// Informações da Ordem de Serviço.
        /// </summary>
        public QueryMovement Information { get; }
        public string MarginTop { get; set; } = "25mm";
        public string MarginLeft { get; set; } = "25mm";
        public string MarginRight { get; set; } = "25mm";
        public string MarginBottom { get; set; } = "5mm";
        private StringBuilder Html { get; set; }
        public DocumentMovement(List<DocumentMovement.Product> Products, QueryMovement Information)
        {
            this.Information = Information;
            this.Products = Products;

            Html = new StringBuilder(
                "<!DOCTYPE html>" +
                "<html>" +
                    "<head>" +
                        "<title>&nbsp;</title>" +
                        "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" +
                        //"<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">" +
                        "<style>" +
                            "p.MsoNormal, li.MsoNormal, div.MsoNormal {" +
                                "margin-top: 0cm;" +
                                "margin-right: 0cm;" +
                                "margin-bottom: 8.0pt;" +
                                "margin-left: 0cm;" +
                                "line-height: 107%;" +
                                "font-size: 11.0pt;" +
                                "padding-top: 0.1cm;" +
                            "}" +
                            ".header, .header-space,.footer, .footer-space {height: 130px;}" +
                            ".header {" +
                            "position: fixed;" +
                            "text-align: center;" +
                            "top: 0;" +
                        "}" +
                            ".footer {position: fixed;bottom: 0;}" +
                        "</style>" +
                        "<style>" +
                        "@page {" +
                            "size: auto;" +
                            "margin-top: @MarginTop;" +
                            "margin-left: @MarginLeft;" +
                            "margin-right: @MarginRight;" +
                            "margin-bottom: @MarginBottom;" +
                        "}" +
                        "</style>" +
                    "</head>" +
                    "<body lang=PT-BR>" +
                    "<table style=\"width:100%;\">" +
                        "<thead>" +
                            "<tr>" +
                                "<td>" +
                                    "<div class=\"header-space\">&nbsp;</div>" +
                                "</td>" +
                            "</tr>" +
                         "</thead>" +
                         "<tbody>" +
                            "<tr>" +
                                "<td>" +
                                    "<div class=\"content\">" +
                                    "<p class=MsoNormal align=center style=\"text-align:center\">" +
                                        "<b><u><span style=\"font-family:'Arial',sans-serif\">LISTAGEM DE MATERIAL</span></u></b>" +
                                    "</p>" +
                                    "<p class=MsoNormal><span style=\"font-size:10.0pt;line-height:107%;font-family: 'Arial',sans-serif\">&nbsp;</span></p>" +
                                    "<table class=MsoTableGrid border=0 cellspacing=0 cellpadding=0 width=633 style=\"width:100%;border-collapse:collapse;border: none\">" +
                                        "<tr>" +
                                            "<td width=133 valign=top style=\"width:99.55pt;background:#C9C9C9;padding: 0cm 5.4pt 0cm 5.4pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\">" +
                                                "<b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\"> Ordem de Serviço </span></b>" +
                                                "</p>" +
                                            "</td>" +
                                            "<td width=104 valign=top style=\"width:77.7pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt\">" +
                                                 "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Data da O.S</span></b></p>" +
                                            "</td>" +
                                            "<td width=70 valign=top style=\"width:52.5pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Situação</span></b></p>" +
                                            "</td>" +
                                            "<td width=132 valign=top style=\"width:99.2pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Matrícula do Fun.</span></b></p>" +
                                            "</td>" +
                                            "<td width=104 valign=top style=\"width:77.95pt;background:#C9C9C9;padding: 0cm 5.4pt 0cm 5.4pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Nome do Fun.</span></b></p>" +
                                            "</td>" +
                                            "<td width=91 valign=top style=\"width:68.0pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Setor</span></b></p>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr style=\"height:3.5pt\">" +
                                            "<td width=133 valign=top style=\"width:99.55pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.WorkOrder}</span></p>" +
                                            "</td>" +
                                            "<td width=132 valign=top style=\"width:99.2pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.WorkOrderDate.ToString("dd/MM/yyyy")}</span></p>" +
                                            "</td>" +
                                            "<td width=104 valign=top style=\"width:77.95pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.Situation}</span></p>" +
                                            "</td>" +
                                            "<td width=91 valign=top style=\"width:68.0pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.RegisterApplication.ToString()}</span></p>" +
                                            "</td>" +
                                            "<td width=70 valign=top style=\"width:52.5pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.SolicitationEmployee}</span></p>" +
                                            "</td>" +
                                            "<td width=104 valign=top style=\"width:77.7pt;padding:0cm 5.4pt 0cm 5.4pt; height:3.5pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{Information.Sector}</span></p>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                    "<p class=MsoNormal><span style=\"font-family:'Arial',sans-serif\">&nbsp;</span></p>" +
                                    "<table class=MsoTableGrid border=0 cellspacing=0 width=633 style=\"width:100%;border-collapse:separate;border: none\">" +
                                        "<tr style=\"height:9.55pt;background-color: #FF9900;\">" +
                                            "<td width=81 valign=top style=\"width:60.65pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt;margin-bottom:1cm;height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Data</span></b></p>" +
                                            "</td>" +
                                            "<td width=44 valign=top style=\"width:32.9pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt; height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"> <b> <span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">SKU</span> </b> </p>" +
                                            "</td>" +
                                            "<td width=109 valign=top style=\" width: 81.45pt; background: #C9C9C9; padding: 0cm 5.4pt 0cm 5.4pt; height: 9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"> <b> <span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Descrição</span> </b> </p>" +
                                            "</td>" +
                                            "<td width=87 valign=top style=\"width:65.6pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt; height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"> <b> <span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Quantidade</span> </b> </p>" +
                                            "</td>" +
                                            "<td width=67 valign=top style=\"width:50.05pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt; height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"> <b> <span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Unidade</span> </b> </p>" +
                                            "</td>" +
                                            "<td width=162 valign=top style=\"width:121.3pt;background:#C9C9C9;padding: 0cm 5.4pt 0cm 5.4pt;height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><b><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Movimentação</span> </b> </p>" +
                                            "</td>" +
                                            "<td width=84 valign=top style=\"width:62.85pt;background:#C9C9C9;padding:0cm 5.4pt 0cm 5.4pt; height:9.55pt\">" +
                                                "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"> <b> <span style=\"font-size:10.0pt;font-family:'Arial',sans-serif; color:black\">Patrimônio</span> </b> </p>" +
                                            "</td>" +
                                        "</tr>" +
                                    "<!--BEGIN ITEMS-->" +
                                            "@LISTOFITEMS" +
                                        "<!--END ITEMS-->" +
                                    "</table>" +
                                        "<p class=MsoNormal><span style=\"font-family:'Arial',sans-serif\">&nbsp;</span></p>" +
                                        "<p class=MsoNormal><span style=\"font-family:'Arial',sans-serif\">&nbsp;</span></p>" +
                                    "</div>" +
                            "</td>" +
                        "</tr>" +
                    "</tbody>" +
                    "<tfoot>" +
                        "<tr>" +
                            "<td>" +
                                "<div class=\"footer-space\">&nbsp;</div>" +
                            "</td>" +
                        "</tr>" +
                    "</tfoot>" +
                    "</table>" +

                        "<div class=\"header\" style=\"width: 100%;\">" +
        "<div>" +
            "<p style=\"text-align:center\">" +
                "<span style=\"font-family:'Arial',sans-serif\">Poder Judiciário do Estado do Rio de Janeiro – PJERJ</span>" +
            "</p>" +
            "<p style=\"text-align:center\">" +
                "<span style=\"font-family:'Arial',sans-serif\">Departamento de Segurança Eletrônica e de Telecomunicações – DETEL</span>" +
            "</p>" +
            "<p style=\"text-align:center\">" +
                "<span style=\"font-family:'Arial',sans-serif\">Setor de Controle de Materiais</span>" +
             "</p>" +
         "</div>" +
     "</div>" +
     "<div class=\"footer\" style=\"width:100%;\">" +
         "<div>" +
             "<p class=MsoNormal style=\"text-align:center;\">" +
                 "<span style=\"font-family:'Arial',sans-serif;\">Não há necessidade de assinatura do Supervisor Técnico, a não ser que haja algum dano na mesma.</span>" +
            "</p>" +
            "<table class=MsoTableGrid border=0 cellspacing=0 cellpadding=0 style=\"width:100%;border-collapse:separate;border-spacing: 3px;border:none\">" +
                "<colgroup>" +
                    "<col span=\"1\" style=\"width: 35%;\">" +
                    "<col span=\"1\" style=\"width: 30%;\">" +
                    "<col span=\"1\" style=\"width: 20%;\">" +
                    "<col span=\"1\" style=\"width: 15%;\">" +
                "</colgroup>" +
                "<tr style=\"height:6.1pt\">" +
                    "<td valign=top style=\"height:6.1pt\">" +
                        "<p class=MsoNormal align=right style=\"margin-bottom:0cm;margin-bottom:.0001pt;text-align:right;line-height:normal\">" +
                            "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Técnico responsável: </span>" +
                              "</p>" +
                          "</td>" +
                          "<td valign=top style=\"border:none;border-bottom:solid windowtext 1.0pt;padding: 0cm 5.4pt 0cm 5.4pt; height: 6.1pt\">" +
                           "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size: 10.0pt; font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                          "</td>" +
                          "<td valign=top style=\"padding:0cm 5.4pt 0cm 5.4pt;height: 6.1pt\">" +
                            "<p class=MsoNormal align=right style=\"margin-bottom: 0cm;margin-bottom: .0001pt;text-align: right;line-height: normal\">" +
                                "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Matrícula:</span>" +
                                "</p>" +
                          "</td>" +
                         "<td valign=top style=\"border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:6.1pt\">" +
                             "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                            "</td>" +
                        "</tr>" +
                        "<tr style=\"height:8.1pt\">" +
                            "<td valign=top style=\"height:6.1pt\">" +
                        "<p class=MsoNormal align=right style=\"margin-bottom:0cm;margin-bottom:.0001pt;text-align:right;line-height:normal\">" +
                            "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Supervisor Técnico: </span>" +
                              "</p>" +
                          "</td>" +
                            "<td valign=top style=\"width:5.0cm;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:8.1pt\">" +
                                  "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                                 "</td>" +
                            "<td valign=top style=\"padding:0cm 5.4pt 0cm 5.4pt;height: 6.1pt\">" +
                                 "<p class=MsoNormal align=right style=\"margin-bottom: 0cm;margin-bottom: .0001pt;text-align: right;line-height: normal\">" +
                                 "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Matrícula:</span>" +
                                 "</p>" +
                            "</td>" +
                               "<td valign=top style=\"width:111.1pt;border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:8.1pt\">" +
                                   "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                                  "</td>" +
                              "</tr>" +
                              "<tr style=\"height:8.1pt\">" +
                                  "<td valign=top style=\"height:8.1pt\">" +
                                      "<p class=MsoNormal align=right style=\"margin-bottom:0cm;margin-bottom:.0001pt;text-align:right;line-height:normal\">" +
                                          "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Setor de Controle de Materiais:</span>" +
                                       "</p>" +
                                   "</td>" +
                                   "<td valign=top style=\"border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:8.1pt\">" +
                                       "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                                      "</td>" +
                                      "<td valign=top style=\"padding:0cm 5.4pt 0cm 5.4pt;height:8.1pt\">" +
                                             "<p class=MsoNormal align=right style=\"margin-bottom:0cm;margin-bottom:.0001pt;text-align:right;line-height:normal\">" +
                                                 "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">Matrícula:</span>" +
                                              "</p>" +
                                          "</td>" +
                                          "<td valign=top style=\"border:none;border-bottom:solid windowtext 1.0pt;padding:0cm 5.4pt 0cm 5.4pt;height:8.1pt\">" +
                                              "<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">" + "<span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">&nbsp;</span>" + "</p>" +
                                             "</td>" +
                                         "</tr>" +
                                     "</table>" +
                                 "</div>" +
                             "</div>" +
                "</body>" +
                "</html>");
            //Html.Append("");
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent +=         "<tr style=\"height:19.1pt\">" +
                                            "<td width=81 valign=top style=\"width:60.65pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.MoveDate.ToString("dd/MM/yyyy")}</span></p>" +
                                            "</td>" +
                                            "<td width=44 valign=top style=\"width:32.9pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Code}</span></p>" +
                                            "</td>" +
                                            "<td width=109 valign=top style=\"width:81.45pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Description}</span></p>" +
                                            "</td>" +
                                            "<td width=87 valign=top style=\"width:65.6pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Quantity}</span></p>" +
                                            "</td>" +
                                            "<td width=67 valign=top style=\"width:50.05pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Unity}</span></p>" +
                                            "</td>" +
                                            "<td width=162 valign=top style=\"width:121.3pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Movement}</span></p>" +
                                            "</td>" +
                                            "<td width=84 valign=top style=\"width:62.85pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Patrimony}</span></p>" +
                                            "</td>" +
                                        "</tr>";
            }
            Html.Replace("@LISTOFITEMS", itemsContent);
            Html.Replace("@MarginTop", MarginTop);
            Html.Replace("@MarginLeft", MarginLeft);
            Html.Replace("@MarginRight", MarginRight);
            Html.Replace("@MarginBottom", MarginBottom);
            return Html.ToString();
        }

        public static ResultSearch Search(string workOrder)
        {
            string userId = string.Empty;
            Monitoring Monitoring;
            InfoUser InfoUser;


            try
            {
                workOrder = System.Uri.EscapeDataString(workOrder);
                Monitoring = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.Server, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                userId = Monitoring.EmployeeId;
            }
            catch
            {
                //If doesn't exist work order, then shows error inside MessageBox 
                MessageBox.Show("Ordem de serviço inexistente.", "Ordem de serviço inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            try
            {
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
            }
            catch
            {
                //HttpRequestException -> BadRequest
                MessageBox.Show("Funcionário não encontrado.", "Funcionário não encontrado", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }


            //Show data in screen
            QueryMovement InformationMovement = new DocumentMovement.QueryMovement()
            {
                Situation = (Monitoring.Situation) ? "FECHADA" : "ABERTA",
                WorkOrder = Monitoring.Work_Order,
                Sector = APIClient.GetData<ModelsLibraryCore.Sector>(new Uri(Helper.Server, $"sector/{Monitoring.RequestingSector}").ToString(), Helper.Authentication).NameSector,
                WorkOrderDate = Monitoring.MovingDate,
                RegisterApplication = int.Parse(InfoUser.Register),
                SolicitationEmployee = InfoUser.Name
            };

            var ProductsToShow = ProductsAtWorkOrder(workOrder);

            return new ResultSearch(ProductsToShow, Monitoring, InfoUser, InformationMovement);
        }
        public static List<DocumentMovement.Product> ProductsAtWorkOrder(string workOrder)
        {

            List<DocumentMovement.Product> ProductsToShow = new List<DocumentMovement.Product>();

            ModelsLibraryCore.MaterialOutput output = null;
            try
            {
                output = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.Server, $"output/workorder/{workOrder}").ToString(), Helper.Authentication);
            }
            catch //Doesn't exist output with that workorder
            { }

            ModelsLibraryCore.MaterialInput input = null;
            try
            {
                input = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.Server, $"devolution/workorder/{workOrder}").ToString(), Helper.Authentication);
            }
            catch //Doesn't exist input with that workorder
            { }

            ManualResetEvent manualReset1 = new ManualResetEvent(false);
            ManualResetEvent manualReset2 = new ManualResetEvent(false);
            ManualResetEvent manualReset3 = new ManualResetEvent(false);
            ManualResetEvent manualReset4 = new ManualResetEvent(false);

            //CONSUMPTERS
            Task.Run(() =>
            {
                if (output != null)
                {

                    foreach (var item in output.ConsumptionProducts.ToList())
                    {
                        //Task.Run for each
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        DocumentMovement.Product product = new DocumentMovement.Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = item.Quantity,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = ""
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset1.Set();
            });

            Task.Run(() =>
            {
                if (input != null)
                {
                    foreach (var item in input.ConsumptionProducts.ToList())
                    {
                        //Task.Run for each
                        {
                            ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                            DocumentMovement.Product product = new DocumentMovement.Product()
                            {
                                Code = infoProduct.Code,
                                Description = infoProduct.Description,
                                Movement = "ENTRADA",
                                Quantity = item.Quantity,
                                Unity = infoProduct.Unity,
                                MoveDate = item.Date,
                                Patrimony = ""
                            };
                            ProductsToShow.Add(product);
                        }
                    }
                }
                manualReset2.Set();
            });

            //PERMANENTS
            Task.Run(() =>
            {
                if (output != null)
                {
                    foreach (var item in output.PermanentProducts.ToList())
                    {
                        //Task.Run for each
                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        DocumentMovement.Product product = new DocumentMovement.Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "SAÍDA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset3.Set();
            });

            Task.Run(() =>
            {
                if (input != null)
                {
                    foreach (var item in input.PermanentProducts.ToList())
                    {
                        //Task.Run for each

                        ModelsLibraryCore.PermanentProduct infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ModelsLibraryCore.ConsumptionProduct infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        DocumentMovement.Product product = new DocumentMovement.Product()
                        {
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Movement = "ENTRADA",
                            Quantity = 1,
                            Unity = infoProduct.Unity,
                            MoveDate = item.Date,
                            Patrimony = infoPermanentProduct.Patrimony
                        };
                        ProductsToShow.Add(product);
                    }
                }
                manualReset4.Set();
            });

            //Esperando todos os sinais
            manualReset1.WaitOne();
            manualReset2.WaitOne();
            manualReset3.WaitOne();
            manualReset4.WaitOne();

            ProductsToShow = ProductsToShow.OrderBy(x => x.MoveDate).ToList();
            return ProductsToShow;
        }

    }
}
