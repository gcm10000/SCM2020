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

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Interação lógica para Closure.xam
    /// </summary>
    public partial class Closure : UserControl
    {
        public Closure()
        {
            InitializeComponent();
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = (DatePickerClosureOSDate.DisplayDate == DateTime.Today) ? DateTime.Now : DatePickerClosureOSDate.DisplayDate;
            var workOrder = TextBoxWorkOrder.Text;
            Task.Run(() =>
            {
                ClosureWO(workOrder, dateTime.Year, dateTime.Month, dateTime.Day);
            });
        }

        private void ClosureWO(string workOrder, int Year, int Month, int Day)
        {
            Uri uriClosure = new Uri(Helper.ServerAPI, $"Monitoring/Closure/{Year}/{Month}/{Day}");
            var result = APIClient.PostData(uriClosure.ToString(), workOrder, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
