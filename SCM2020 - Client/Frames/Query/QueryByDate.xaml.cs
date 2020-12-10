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
using System.Threading.Tasks;
using System.Windows.Threading;

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
                ENTRADA DE DEVOLUÇÃO -> AUXILIARPRODUCT INPUT
                TOTAL DE ESTOQUE -> ENTRADA NO ESTOQUE + ENTRADA DE DEVOLUÇÃO
                SAÍDA -> AUXILIARPRODUCT OUTPUT
                SALDO ATUAL -> TOTAL DE ESTOQUE - SAÍDA
                ESTOQUE MÍNIMO -> GENERALPRODUCT
                ESTOQUE MÁXIMO -> GENERALPRODUCT
                UNIDADE -> GENERALPRODUCT
             */
            var initialDate = InitialDate.SelectedDate.Value;
            var finalDate = FinalDate.SelectedDate.Value;
            Task.Run(() => Search(initialDate, finalDate));
            
        }

        private void Search(DateTime initialDate, DateTime finalDate)
        {
            ButtonEnable(false);
            this.ShowByDateDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ShowByDateDataGrid.Items.Clear(); }));


            var materialInputByVendorInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.ServerAPI, $"input/Date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialOutputInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.ServerAPI, $"output/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialDevolutionInDate = APIClient.GetData<List<ModelsLibraryCore.AuxiliarConsumption>>(new Uri(Helper.ServerAPI, $"devolution/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            InitialDateTime = initialDate.Date;
            FinalDateTime = finalDate.Date;

            products = new List<QueryByDateDocument.Product>();
            foreach (var item in materialInputByVendorInDate)
            {
                //ADD INFO
                if (!products.Any(x => (x.ProductId == item.ProductId) && (x.WorkOrder == item.WorkOrder)))
                {
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        ProductId = product.Id,
                        StockEntry = item.Quantity, //ENTRADA POR FORNECEDOR
                        Unity = product.Unity,
                        WorkOrder = item.WorkOrder
                        //StockDevolution =
                        //Output =
                    });
                }
                else
                {
                    QueryByDateDocument.Product product = products.Single(x => x.ProductId == item.ProductId);
                    product.StockEntry += item.Quantity;
                }
            }

            foreach (var item in materialOutputInDate)
            {
                //ADD INFO
                if (!products.Any(x => (x.ProductId == item.ProductId) && (x.WorkOrder == item.WorkOrder)))
                {
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        ProductId = product.Id,
                        //StockDevolution =
                        //StockEntry = item.Quantity,
                        Unity = product.Unity,
                        Output = item.Quantity,
                        WorkOrder = item.WorkOrder
                    });
                }
                else
                {
                    QueryByDateDocument.Product product = products.Single(x => (x.ProductId == item.ProductId) && (x.WorkOrder == item.WorkOrder));
                    product.Output += item.Quantity;
                }
            }

            foreach (var item in materialDevolutionInDate)
            {
                //ADD INFO
                if (!products.Any(x => (x.ProductId == item.ProductId) && (x.WorkOrder == item.WorkOrder)))
                {
                    var product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    products.Add(new QueryByDateDocument.Product()
                    {
                        Code = product.Code,
                        Description = product.Description,
                        MinimumStock = product.MininumStock,
                        MaximumStock = product.MaximumStock,
                        ProductId = product.Id,
                        Unity = product.Unity,
                        StockDevolution = item.Quantity,
                        WorkOrder = item.WorkOrder
                        //Output =
                        //StockEntry =
                    });
                }
                else
                {
                    QueryByDateDocument.Product product = products.Single(x => (x.ProductId == item.ProductId) && (x.WorkOrder == item.WorkOrder));
                    product.StockDevolution += item.Quantity;
                }
            }

            foreach (var product in products)
            {
                this.ShowByDateDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ShowByDateDataGrid.Items.Add(product); }));
            }

            ButtonEnable(true);
        }

        private void ButtonEnable(bool isEnable)
        {
            this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = isEnable; }));
            this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = isEnable; }));
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
