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
using WebAssemblyLibrary;

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
            //Captura a ordem de serviço escrita pelo usuário
            string workOrder = OSTextBox.Text;

            MessageBoxResult resultBox = MessageBox.Show("Deseja realmente abrir a ordem de serviço?", "Você tem certeza disso", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //Se o usuário selecionou o botão sim...
            if (resultBox == MessageBoxResult.Yes)
            {
                Task.Run(() =>
                {
                    //Checar se a ordem de serviço existe
                    try
                    {
                        workOrder = System.Uri.EscapeDataString(workOrder);
                        bool SituationMonitoring = APIClient.GetData<bool>(new Uri(Helper.Server, $"Monitoring/CheckWorkOrder/{workOrder}").ToString(), Helper.Authentication);
                        if (SituationMonitoring)
                        {
                            //Abrir ordem de serviço
                            var result = APIClient.GetData<string>(new Uri(Helper.Server, $"monitoring/reopen/{workOrder}").ToString(), Helper.Authentication);
                            //Exibe mensagem recebida do recebidor ao usuário.
                            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ordem de serviço encontra-se aberta.", "Ordem de serviço aberta", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ordem de serviço inexistente", "Ordem de serviço inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                });
            }
        }
    }
}
