using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        DocumentMovement.QueryMovement InformationQuery = null;
        public Movement()
        {
            InitializeComponent();
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
            //Monitoring = null;
            //InfoUser = null;

            DocumentMovement.ResultSearch resultSearch = DocumentMovement.Search(workOrder);
            if (resultSearch == null)
            {
                this.Export_Button.IsEnabled = true;
                this.Print_Button.IsEnabled = true;

                return;
            }
            DocumentMovement.QueryMovement InformationQuery = resultSearch.InformationQuery;

            OSText.Text = InformationQuery.WorkOrder;
            RegisterApplicationTextBox.Text = InformationQuery.RegisterApplication.ToString();
            ApplicationTextBox.Text = InformationQuery.SolicitationEmployee;
            SectorTextBox.Text = InformationQuery.Sector;
            SituationTextBox.Text = InformationQuery.Situation;
            ServiceLocalizationTextBox.Text = InformationQuery.ServiceLocalizationTextBox;
            WorkOrderDateDatePicker.SelectedDate = InformationQuery.WorkOrderDate;

            ProductsToShow = resultSearch.QueryMovement;
            foreach (var product in ProductsToShow)
            {
                ProductMovementDataGrid.Items.Add(product);
            }
            this.Export_Button.IsEnabled = true;
            this.Print_Button.IsEnabled = true;
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
            SCM2020___Client.Templates.Query.Movement template = new SCM2020___Client.Templates.Query.Movement(WorkOrder: InformationQuery.WorkOrder,
                RegisterApplication: InformationQuery.RegisterApplication,
                Application: InformationQuery.SolicitationEmployee,
                Sector: InformationQuery.Sector,
                Situation: InformationQuery.Situation,
                ServiceLocalization: InformationQuery.ServiceLocalizationTextBox,
                WorkOrderDate: InformationQuery.WorkOrderDate,
                ClosureWorkOrder: InformationQuery.ClosingDate,
                Products: ProductsToShow);
            var result = template.RenderizeHtml();
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
            SCM2020___Client.Templates.Query.Movement template = new SCM2020___Client.Templates.Query.Movement(WorkOrder: InformationQuery.WorkOrder,
                RegisterApplication: InformationQuery.RegisterApplication,
                Application: InformationQuery.SolicitationEmployee,
                Sector: InformationQuery.Sector,
                Situation: InformationQuery.Situation,
                ServiceLocalization: InformationQuery.ServiceLocalizationTextBox,
                WorkOrderDate: InformationQuery.WorkOrderDate,
                ClosureWorkOrder: InformationQuery.ClosingDate,
                Products: ProductsToShow);
            var result = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(result);
        }
    }

}
