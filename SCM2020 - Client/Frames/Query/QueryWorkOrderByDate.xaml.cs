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
    /// Interação lógica para QueryWorkOrderByDate.xam
    /// </summary>
    public partial class QueryWorkOrderByDate : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        SCM2020___Client.Templates.Query.QueryWorkOrderByDate ResultQueryWorkOrder;
        public QueryWorkOrderByDate()
        {
            InitializeComponent();
        }

        private void InitialDate_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox1 = (TextBox)InitialDate.Template.FindName("PART_TextBox", InitialDate);
            textBox1.Background = InitialDate.Background;
        }

        private void FinalDate_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox2 = (TextBox)FinalDate.Template.FindName("PART_TextBox", FinalDate);
            textBox2.Background = FinalDate.Background;
        }

        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.QueryWorkOrderByDate workOrderSelected = ((Models.QueryWorkOrderByDate)this.DataGridShowByDate.GetObjectFromDataGridRow());

            Helper.WorkOrderByPass = workOrderSelected.WorkOrder;
            FrameWindow frame = new FrameWindow(new Uri("Frames/Query/Movement.xaml", UriKind.Relative));
            frame.Show();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var initialDay = this.InitialDate.SelectedDate.Value.Day;
            var initialMonth = this.InitialDate.SelectedDate.Value.Month;
            var initialYear = this.InitialDate.SelectedDate.Value.Year;

            var finalDay = this.FinalDate.SelectedDate.Value.Day;
            var finalMonth = this.FinalDate.SelectedDate.Value.Month;
            var finalYear = this.FinalDate.SelectedDate.Value.Year;

            ButtonsStatus(false);
            Search(initialDay, initialMonth, initialYear, finalDay, finalMonth, finalYear);
        }
        private void Search(int initialDay, int initialMonth, int initialYear, int finalDay, int finalMonth, int finalYear)
        {
            Task.Run(() =>
            {
                this.DataGridShowByDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridShowByDate.Items.Clear(); }));
                var result = APIClient.GetData<List<ModelsLibraryCore.Monitoring>>(new Uri(Helper.ServerAPI, $"Monitoring/SearchByDate/{initialDay}-{initialMonth}-{initialYear}/{finalDay}-{finalMonth}-{finalYear}").ToString(), Helper.Authentication);
                List<Models.QueryWorkOrderByDate> dataQuery = new List<Models.QueryWorkOrderByDate>();
                foreach (var item in result)
                {
                    SCM2020___Client.Models.QueryWorkOrderByDate workorder = new SCM2020___Client.Models.QueryWorkOrderByDate()
                    {
                        WorkOrder = item.Work_Order,
                        MovingDate = item.MovingDate,
                        ClosingDate = item.ClosingDate
                    };
                    dataQuery.Add(workorder);
                }
                this.DataGridShowByDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridShowByDate.ItemsSource = dataQuery; }));

                ResultQueryWorkOrder = new Templates.Query.QueryWorkOrderByDate(dataQuery, new DateTime(initialYear, initialMonth, initialDay), new DateTime(finalYear, finalMonth, finalDay));
                if ((dataQuery.Count > 0) && (dataQuery != null))
                    ButtonsStatus(true);
            });
        }
        private void ButtonsStatus(bool isEnable)
        {
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = isEnable; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = isEnable; }));
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

        private void ShowByDateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;

            Document = ResultQueryWorkOrder.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            Document = ResultQueryWorkOrder.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void DataGridShowByDate_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
        }
    }
}
