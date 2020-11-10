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
            DateTime dateTime = (ClosureOSDateTextBox.DisplayDate == DateTime.Today) ? DateTime.Now : ClosureOSDateTextBox.DisplayDate;
            var workOrder = OSTextBox.Text;
            Uri uriClosure = new Uri(Helper.ServerAPI, $"Monitoring/Closure/{dateTime.Year}/{dateTime.Month}/{dateTime.Day}");
            Task.Run(() =>
            {
                var result = APIClient.PostData(uriClosure.ToString(), workOrder, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            }).Wait();
        }
    }
}
