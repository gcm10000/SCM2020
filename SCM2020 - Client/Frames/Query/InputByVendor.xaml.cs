using Microsoft.VisualBasic;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Channels;
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
    /// Interação lógica para InputByVendor.xam
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        SCM2020___Client.Templates.Query.InputByVendor inputDocument;


        //NOTA FISCAL, DATA DA MOVIMENTAÇÃO, FORNECEDOR, FUNCIONÁRIO
        public InputByVendor()
        {
            InitializeComponent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string invoice = TxtSearch.Text;
            Task.Run(() =>
            {
                if (e.Key == Key.Enter)
                    Search(invoice);
            });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string invoice = TxtSearch.Text;
            Task.Run(() => 
            { 
                Search(invoice);
            });
        }

        private void Search(string invoice)
        {
            //Zerar todos os dados anteriores...
            Clear();

            inputDocument = new Templates.Query.InputByVendor(invoice);

            foreach (var product in inputDocument.Products)
            {
                this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ProductMovementDataGrid.Items.Add(product); }));
            }

            this.InvoiceText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InvoiceText.Text = inputDocument.Invoice; }));
            this.VendorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.VendorTextBox.Text = inputDocument.Vendor; }));
            this.RegistrationSCMTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegistrationSCMTextBox.Text = inputDocument.SCMRegistration; }));
            this.SCMEmployeeTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SCMEmployeeTextBox.Text = inputDocument.SCMEmployee; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = this.WorkOrderDateDatePicker.DisplayDate = inputDocument.InvoiceDate; }));

            AllowButtons();
        }
        private void Clear()
        {
            inputDocument = null;
            this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = false; }));
            this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = false; }));
            this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ProductMovementDataGrid.Items.Clear(); }));
        }
        private void AllowButtons()
        {
            this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = true; }));
            this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = true; }));
        }
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            //DocumentInputByVendor template = new DocumentInputByVendor(ProductsToShow, info);
            //Document = template.RenderizeHtml();
            //this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            //this.webBrowser.NavigateToString(Document);
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            //DocumentInputByVendor template = new DocumentInputByVendor(ProductsToShow, info);
            //var result = template.RenderizeHtml();
            //this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            //this.webBrowser.NavigateToString(result);
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
