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

            if (Helper.WorkOrderByPass != string.Empty)
            {
                TxtSearch.Text = Helper.WorkOrderByPass;
                Search(Helper.WorkOrderByPass);
                Helper.WorkOrderByPass = string.Empty;
            }

        }
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Search(TxtSearch.Text); //TxtSearch.Text equivale a workOrder
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search(TxtSearch.Text); //TxtSearch.Text equivale a workOrder

        }
        private void Search(string workOrder)
        {
            //Zerar todos os dados anteriores...
            this.Export_Button.IsEnabled = false;
            this.Print_Button.IsEnabled = false;
            ProductMovementDataGrid.Items.Clear();
            ProductsToShow = null;

            OSText.Text = string.Empty;
            RegisterApplicationTextBox.Text = string.Empty;
            ApplicationTextBox.Text = string.Empty;
            SectorTextBox.Text = string.Empty;
            SituationTextBox.Text = string.Empty;
            ServiceLocalizationTextBox.Text = string.Empty;
            WorkOrderDateDatePicker.SelectedDate = null;
            ClosureOSDatePicker.SelectedDate = null;

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
                this.Export_Button.IsEnabled = true;
                this.Print_Button.IsEnabled = true;

                return;
            }

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

            var result = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(result);

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

            var result = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(result);
        }
    }

}
