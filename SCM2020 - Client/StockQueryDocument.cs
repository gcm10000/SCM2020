using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client
{
    public class StockQueryDocument
    {
        private List<ModelsLibraryCore.ConsumptionProduct> Products;
        private StringBuilder Html;
        public string MarginTop { get; set; } = "25mm";
        public string MarginLeft { get; set; } = "25mm";
        public string MarginRight { get; set; } = "25mm";
        public string MarginBottom { get; set; } = "5mm";
        public StockQueryDocument(List<ModelsLibraryCore.ConsumptionProduct> Products)
        {
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
        }
        public string RenderizeHtml()
        {
            string itemsContent = string.Empty;
            foreach (var product in Products)
            {
                itemsContent += "<tr style=\"height:19.1pt\">" +
                                            "<td width=81 valign=top style=\"width:60.65pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Code}</span></p>" +
                                            "</td>" +
                                            "<td width=44 valign=top style=\"width:32.9pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Description}</span></p>" +
                                            "</td>" +
                                            "<td width=109 valign=top style=\"width:81.45pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.MininumStock}</span></p>" +
                                            "</td>" +
                                            "<td width=87 valign=top style=\"width:65.6pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Stock}</span></p>" +
                                            "</td>" +
                                            "<td width=87 valign=top style=\"width:65.6pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.MaximumStock}</span></p>" +
                                            "</td>" +
                                            "<td width=67 valign=top style=\"width:50.05pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Unity}</span></p>" +
                                            "</td>" +
                                            "<td width=67 valign=top style=\"width:50.05pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Localization}</span></p>" +
                                            "</td>" +
                                            "<td width=67 valign=top style=\"width:50.05pt;padding:0cm 5.4pt 0cm 5.4pt; height:19.1pt\">" +
                                                $"<p class=MsoNormal style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height: normal\"><span style=\"font-size:10.0pt;font-family:'Arial',sans-serif\">{product.Group}</span></p>" +
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

    }
}
