using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using RazorEngine.Compilation.ImpromptuInterface;
using SCM2020___Client.Templates.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para Movement.xam
    /// </summary>

    public partial class Movement : UserControl
    {
        List<Templates.Query.Movement.Product> ProductsToShow = null;
        WebBrowser webBrowser = Helper.MyWebBrowser;
        SCM2020___Client.Templates.Query.Movement ResultMovement = null;
        public Movement()
        {
            InitializeComponent();

            if (!((Helper.WorkOrderByPass == string.Empty) || (Helper.WorkOrderByPass == null)))
            {
                TxtSearch.Text = Helper.WorkOrderByPass;
                Search(Helper.WorkOrderByPass);
                Helper.WorkOrderByPass = string.Empty;
            }

        }
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string workOrder = TxtSearch.Text;
                Task.Run(() => Search(workOrder));
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = TxtSearch.Text;
            Task.Run(() => Search(workOrder));
        }
        private void Search(string workOrder)
        {
            //Zerar todos os dados anteriores...
            this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = false; }));
            this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = false; }));
            this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ProductMovementDataGrid.Items.Clear(); }));
            
            this.OSText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSText.Text = string.Empty; }));
            this.RegisterApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicationTextBox.Text = string.Empty; }));
            this.ApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicationTextBox.Text = string.Empty; }));
            this.SectorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SectorTextBox.Text = string.Empty; }));
            this.SituationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SituationTextBox.Text = string.Empty; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = string.Empty; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.Text = string.Empty; }));
            this.ClosureOSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ClosureOSDatePicker.SelectedDate = null; }));


            try
            {
                ResultMovement = new SCM2020___Client.Templates.Query.Movement(workOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (ResultMovement == null)
            {
                this.Export_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Export_Button.IsEnabled = true; }));
                this.Print_Button.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Print_Button.IsEnabled = true; }));

                return;
            }

            this.OSText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSText.Text = ResultMovement.WorkOrder; }));
            this.RegisterApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicationTextBox.Text = ResultMovement.RegisterApplication.ToString(); }));
            this.ApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicationTextBox.Text = ResultMovement.Application; }));
            this.SectorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SectorTextBox.Text = ResultMovement.Sector; }));
            this.SituationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SituationTextBox.Text = ResultMovement.Situation; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = ResultMovement.ServiceLocalization; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = ResultMovement.WorkOrderDate; }));
            this.ClosureOSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ClosureOSDatePicker.SelectedDate = ResultMovement.ClosureWorkOrder; }));


            OSText.Text = ResultMovement.WorkOrder;
            RegisterApplicationTextBox.Text = ResultMovement.RegisterApplication.ToString();
            ApplicationTextBox.Text = ResultMovement.Application;
            SectorTextBox.Text = ResultMovement.Sector;
            SituationTextBox.Text = ResultMovement.Situation;
            ServiceLocalizationTextBox.Text = ResultMovement.ServiceLocalization;
            WorkOrderDateDatePicker.SelectedDate = ResultMovement.WorkOrderDate;
            ClosureOSDatePicker.SelectedDate = ResultMovement.ClosureWorkOrder;

            ProductsToShow = ResultMovement.Products;
            foreach (var product in ProductsToShow)
            {
                ProductMovementDataGrid.Items.Add(product);
            }
            this.Export_Button.IsEnabled = true;
            this.Print_Button.IsEnabled = true;

            ProductMovementDataGrid.UnselectAll();
        }
        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;

            Document = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);

        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
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
        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;

            Document = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }
    }

}
