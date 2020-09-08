using Microsoft.VisualBasic;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para QueryByPatrimony.xam
    /// </summary>
    public partial class QueryByPatrimony : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        Templates.Query.QueryByPatrimony ResultQueryByPatrimony = null;
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;
        public QueryByPatrimony()
        {
            InitializeComponent();
        }

        private void QueryDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string patrimony = TxtSearch.Text;
            Task.Run(() => Search(patrimony));
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string patrimony = TxtSearch.Text;
                Task.Run(() => Search(patrimony));
            }
        }
        private void Search(string patrimony)
        {
            ClearData();

            if (patrimony == string.Empty)
                return;
            patrimony = System.Uri.EscapeDataString(patrimony);
            var result = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.Server, $"PermanentProduct/Search/{patrimony}").ToString(), Helper.Authentication);
            List<SCM2020___Client.Models.QueryByPatrimony> listQuery = new List<SCM2020___Client.Models.QueryByPatrimony>();
            foreach (var item in result)
            {
                //Pega informações referente ao produto
                var informationProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.InformationProduct}").ToString(), Helper.Authentication);
                var workOrder = System.Uri.EscapeDataString(item.WorkOrder);
                //Pega informações referente a ordem de serviço
                var informationOS = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.Server, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                //Pega informações referente ao técnico
                var InfoUser = APIClient.GetData<ModelsLibraryCore.InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{informationOS.EmployeeId}").ToString(), Helper.Authentication);

                SCM2020___Client.Models.QueryByPatrimony product = new SCM2020___Client.Models.QueryByPatrimony()
                {
                    Code = informationProduct.Code,
                    Description = informationProduct.Description,
                    Patrimony = item.Patrimony,
                    WorkOrder = item.WorkOrder,
                    Employee = InfoUser.Name
                };
                
                //Adiciona no datagrid
                this.QueryDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.QueryDataGrid.Items.Add(product); }));
            }
            try
            {
                ResultQueryByPatrimony = new Templates.Query.QueryByPatrimony(listQuery);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ClearData()
        {
            this.QueryDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { QueryDataGrid.Items.Clear(); }));
        }
        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;

            Document = ResultQueryByPatrimony.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

            PrintORExport = true;

            Document = ResultQueryByPatrimony.RenderizeHtml();
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
                    p.StartInfo.FileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Exporter\\document-exporter.exe");
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
