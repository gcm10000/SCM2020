using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
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

namespace SCM2020___Client.Frames.Movement
{
    /// <summary>
    /// Interação lógica para Reopen.xam
    /// </summary>
    public partial class Reopen : UserControl
    {
        public Reopen()
        {
            InitializeComponent();
        }

        private void BtnReopen_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = OSTextBox.Text;
            MessageBoxResult resultBox = MessageBox.Show("Deseja realmente abrir a ordem de serviço?", "Você tem certeza disso", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultBox == MessageBoxResult.Yes)
            {
                Task.Run(() =>
                {
                    ModelsLibraryCore.Monitoring Monitoring;
                    //Checar se a ordem de serviço existe
                    try
                    {
                        workOrder = System.Uri.EscapeDataString(workOrder);
                        Monitoring = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.Server, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                    }
                    catch
                    {
                        MessageBox.Show("Ordem de serviço inexistente.", "Ordem de serviço inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                });
            }
        }
    }
}
