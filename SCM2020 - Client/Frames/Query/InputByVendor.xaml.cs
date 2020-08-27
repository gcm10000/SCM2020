﻿using Microsoft.VisualBasic;
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
        SCM2020___Client.Templates.Query.InputByVendor template;


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

            template = new Templates.Query.InputByVendor(invoice);
            if (template.Products == null)
                return;
            foreach (var product in template.Products)
            {
                this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ProductMovementDataGrid.Items.Add(product); }));
            }

            this.InvoiceText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InvoiceText.Text = template.Invoice; }));
            this.VendorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.VendorTextBox.Text = template.Vendor; }));
            this.RegistrationSCMTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegistrationSCMTextBox.Text = template.SCMRegistration; }));
            this.SCMEmployeeTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SCMEmployeeTextBox.Text = template.SCMEmployee; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = this.WorkOrderDateDatePicker.DisplayDate = template.InvoiceDate; }));

            AllowButtons();
        }
        private void Clear()
        {
            template = null;
            this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = false; }));
            this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = false; }));
            this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ProductMovementDataGrid.Items.Clear(); }));

            this.InvoiceText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InvoiceText.Text = string.Empty; }));
            this.VendorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.VendorTextBox.Text = string.Empty; }));
            this.RegistrationSCMTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegistrationSCMTextBox.Text = string.Empty; }));
            this.SCMEmployeeTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SCMEmployeeTextBox.Text = string.Empty; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = null; this.WorkOrderDateDatePicker.DisplayDate = DateTime.Today; }));

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
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            //DocumentInputByVendor template = new DocumentInputByVendor(ProductsToShow, info);
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
