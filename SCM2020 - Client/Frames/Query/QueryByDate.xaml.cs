using Microsoft.EntityFrameworkCore.Internal;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Linq;
using Path = System.IO.Path;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para screen.xam
    /// </summary>
    public partial class QueryByDate : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        List<QueryByDateDocument.Product> products = null;

        public QueryByDate()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //ModelsLibraryCore.RequestingClient.APIClient.GetData<>()
            /*
             CODIGO -> GENERALPRODUCT
             DESC -> GENERALPRODUCT
             ENTRADA NO ESTOQUE -> AUXILIARPRODUCT INPUT BY VENDOR
             ESTOQUE ATUAL -> GENERALPRODUCT
             SAÍDA -> AUXILIARPRODUCT OUTPUT
             ESTOQUE MÍNIMO -> GENERALPRODUCT
             ESTOQUE MÁXIMO -> GENERALPRODUCT
             UNIDADE -> GENERALPRODUCT

             */
            //int id = 0;
            var initialDate = InitialDate.SelectedDate.Value;
            var finalDate = FinalDate.SelectedDate.Value;
            var materialInputByVendorInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.Server, $"input/Date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialOutputInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.Server, $"output/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialDevolutionInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.Server, $"devolution/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            InitialDateTime = initialDate.Date;
            FinalDateTime = FinalDate.DisplayDate;

            products = new List<QueryByDateDocument.Product>();
            foreach (var item in materialInputByVendorInDate)
            {
                //ADD INFO
                //if (!products.Any(x => x.ProductId == item.ProductId))
                //{
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        Stock = product.Stock,
                        ProductId = product.Id,
                        StockEntry = item.Quantity,
                        Unity = product.Unity,
                        //Output =
                    });
                //}
                //else
                //{
                //    QueryByDateDocument.Product product = products.Single(x => x.ProductId == item.ProductId);
                //    product.StockEntry = item.Quantity;
                //}
            }

            foreach (var item in materialOutputInDate)
            {
                //ADD INFO
                if (!products.Any(x => x.ProductId == item.ProductId))
                {
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        Stock = product.Stock,
                        ProductId = product.Id,
                        //StockEntry = item.Quantity,
                        Unity = product.Unity,
                        Output = item.Quantity
                    });
                }
                else
                {
                    QueryByDateDocument.Product product = products.Single(x => x.ProductId == item.ProductId);
                    product.Output += item.Quantity;
                }
            }

            foreach (var item in materialDevolutionInDate)
            {
                //ADD INFO
                if (!products.Any(x => x.ProductId == item.ProductId))
                {
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        Stock = product.Stock,
                        ProductId = product.Id,
                        StockEntry = item.Quantity,
                        Unity = product.Unity
                        //Output = item.Quantity

                    });
                }
                else
                {
                    QueryByDateDocument.Product product = products.Single(x => x.ProductId == item.ProductId);
                    product.StockEntry += item.Quantity;
                }
            }

            //APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"/generalproduct/{id}").ToString(), Helper.Authentication);
        }

        private void ShowByDateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;
        DateTime InitialDateTime;
        DateTime FinalDateTime;

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;

            QueryByDateDocument template = new QueryByDateDocument(InitialDateTime, FinalDateTime, products);
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            QueryByDateDocument template = new QueryByDateDocument(InitialDateTime, FinalDateTime, products);
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Helper.SetOptionsToPrint();
            if (PrintORExport)
            {
                webBrowser.PrintDocument();
            }
            else
            {
                string printer = Helper.GetPrinter("PDF");
                string tempFile = string.Empty;
                try
                {

                    tempFile = Helper.GetTempFilePathWithExtension(".tmp");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(tempFile, true))
                    {
                        file.Write(Document);
                        file.Flush();
                    }

                    //"f=" The input file
                    //"p=" The temporary default printer
                    //"d|delete" Delete file when finished
                    var p = new Process();
                    p.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "Exporter\\document-exporter.exe");
                    //Fazer com que o document-exporter apague o arquivo após a impressão. Ao invés de utilizar finally. Motivo é evitar que o arquivo seja apagado antes do Document-Exporter possa lê-lo.
                    p.StartInfo.Arguments = $"-p=\"{printer}\" -f=\"{tempFile}\" -d";
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro durante exportação", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            webBrowser.LoadCompleted -= WebBrowser_LoadCompleted;
        }
    }
}
