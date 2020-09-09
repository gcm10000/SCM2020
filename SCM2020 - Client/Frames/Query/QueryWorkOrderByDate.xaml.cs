using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
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

        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Garantir que a linha selecionada foi clicada e não um espaço em branco
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null) 
                return;

            SCM2020___Client.Models.QueryWorkOrderByDate workOrder = row.Item as SCM2020___Client.Models.QueryWorkOrderByDate;
            Helper.WorkOrderByPass = workOrder.WorkOrder;
            FrameWindow frame = new FrameWindow(new Uri("Frames/Query/Movement.xaml", UriKind.Relative));
            frame.Show();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //QueryWorkOrderByDateDataGrid 
            var initialDay = this.InitialDate.DisplayDate.Day;
            var initialMonth = this.InitialDate.DisplayDate.Month;
            var initialYear = this.InitialDate.DisplayDate.Year;

            var finalDay = this.FinalDate.DisplayDate.Day;
            var finalMonth = this.FinalDate.DisplayDate.Month;
            var finalYear = this.FinalDate.DisplayDate.Year;

            Search(initialDay, initialMonth, initialYear, finalDay, finalMonth, finalYear);
        }
        private void Search(int initialDay, int initialMonth, int initialYear, int finalDay, int finalMonth, int finalYear)
        {
            Task.Run(() =>
            {
                this.ShowByDateDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ShowByDateDataGrid.Items.Clear(); }));
                var result = APIClient.GetData<List<ModelsLibraryCore.Monitoring>>(new Uri(Helper.Server, $"monitoring/searchbydate/{initialDay}/{initialMonth}/{initialYear}/{finalDay}/{finalMonth}/{finalYear}").ToString(), Helper.Authentication);
                List<Models.QueryWorkOrderByDate> dataQuery = new List<Models.QueryWorkOrderByDate>();
                foreach (var item in result)
                {
                    SCM2020___Client.Models.QueryWorkOrderByDate workorder = new SCM2020___Client.Models.QueryWorkOrderByDate()
                    {
                        WorkOrder = item.Work_Order,
                        Date = item.MovingDate
                    };
                    this.ShowByDateDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ShowByDateDataGrid.Items.Add(workorder); }));
                    dataQuery.Add(new Models.QueryWorkOrderByDate() { Date = item.MovingDate, WorkOrder = item.Work_Order });
                }
                ResultQueryWorkOrder = new Templates.Query.QueryWorkOrderByDate(dataQuery, new DateTime(initialYear, initialMonth, initialDay), new DateTime(finalYear, finalMonth, finalDay));
            });
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            Document = ResultQueryWorkOrder.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }


        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;

            Document = ResultQueryWorkOrder.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }
        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        private void ShowByDateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
