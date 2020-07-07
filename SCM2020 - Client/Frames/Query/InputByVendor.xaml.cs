using Microsoft.VisualBasic;
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

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para InputByVendor.xam
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        List<DocumentInputByVendor.Product> ProductsToShow = null;
        DocumentInputByVendor.QueryInputByVendor info = null;


        //NOTA FISCAL v, DATA DA MOVIMENTAÇÃO v, FORNECEDOR, FUNCIONÁRIO v
        public InputByVendor()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string invoice = TxtSearch.Text;
            if (e.Key == Key.Enter)
                Search(invoice);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string invoice = TxtSearch.Text;
            Search(invoice);
        }

        private void Search(string invoice)
        {
            //Zerar todos os dados anteriores...
            this.Export_Button.IsEnabled = false;
            this.Print_Button.IsEnabled = false;

            DocumentInputByVendor.ResultSearch resultSearch = null;
            var t = Task.Run(() => resultSearch = DocumentInputByVendor.Search(invoice));
            t.Wait();
            var InformationQuery = resultSearch.InformationMovement;
            info = InformationQuery;
            InvoiceText.Text = InformationQuery.Invoice;
            VendorTextBox.Text = InformationQuery.Vendor;
            RegistrationSCMTextBox.Text = InformationQuery.SCMRegistration;
            SCMEmployeeTextBox.Text = InformationQuery.SCMEmployee;
            WorkOrderDateDatePicker.DisplayDate = InformationQuery.InvoiceDate;


            ProductsToShow = resultSearch.Products;
            info = resultSearch.InformationMovement;
            foreach (var product in ProductsToShow)
            {
                ProductMovementDataGrid.Items.Add(product);
            }

            this.Export_Button.IsEnabled = true;
            this.Print_Button.IsEnabled = true;
        }

        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            DocumentInputByVendor template = new DocumentInputByVendor(ProductsToShow, info);
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            DocumentInputByVendor template = new DocumentInputByVendor(ProductsToShow, info);
            var result = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(result);
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

        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
