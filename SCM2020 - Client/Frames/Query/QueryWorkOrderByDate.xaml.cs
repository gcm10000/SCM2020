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
        public class QueryWorkOrderByDateDataGrid
        {
            public string WorkOrder { get; set; }
            public DateTime Date { get; set; }
        }
        public QueryWorkOrderByDate()
        {
            InitializeComponent();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Garantir que a linha selecionada foi clicada e não um espaço em branco
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            //row.Item
            if (row == null) return;
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

            Task.Run(() =>
            {
                var result = APIClient.GetData<List<ModelsLibraryCore.Monitoring>>(new Uri(Helper.Server, $"monitoring/searchbydate/{initialDay}/{initialMonth}/{initialYear}/{finalDay}/{finalMonth}/{finalYear}").ToString(), Helper.Authentication);

                foreach (var item in result)
                {
                    QueryWorkOrderByDateDataGrid workorder = new QueryWorkOrderByDateDataGrid()
                    {
                        WorkOrder = item.Work_Order,
                        Date = item.MovingDate
                    };
                    this.ShowByDateDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ShowByDateDataGrid.Items.Add(workorder); }));
                }
            });
        }
    }
}
