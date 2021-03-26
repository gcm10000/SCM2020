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

        private void ButtonOpenWO_Click(object sender, RoutedEventArgs e)
        {
            //Captura a ordem de serviço escrita pelo usuário
            string workOrder = TextBoxWorkOrder.Text;
            Task.Run(() => 
            {
                OpenWorkOrder(workOrder);
            });
        }

        private void OpenWorkOrder(string workOrder)
        {
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
                        bool SituationMonitoring = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"Monitoring/CheckWorkOrder/{workOrder}").ToString(), Helper.Authentication);
                        if (SituationMonitoring)
                        {
                            //Abrir ordem de serviço
                            var result = APIClient.GetData<string>(new Uri(Helper.ServerAPI, $"monitoring/reopen/{workOrder}").ToString(), Helper.Authentication);
                            //Exibe mensagem recebida do recebidor ao usuário.
                            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ordem de serviço encontra-se aberta.", "Ordem de serviço aberta", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ordem de serviço inexistente", "Ordem de serviço inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                });
            }
        }

        private void TextBoxWorkOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Captura a ordem de serviço escrita pelo usuário
                string workOrder = TextBoxWorkOrder.Text;
                Task.Run(() => 
                {
                    if (StatusWO(workOrder))
                    {
                        OpenWorkOrder(workOrder); 
                    }
                });
            }
        }

        private void ScrollViewerInfo_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private bool StatusWO(string workOrder)
        {
            workOrder = System.Uri.EscapeDataString(workOrder);

            Uri uriExistsWorkOrder = new Uri(Helper.ServerAPI, $"Monitoring/ExistsWorkOrder/{workOrder}");
            var result = APIClient.GetData<bool>(uriExistsWorkOrder.ToString(), Helper.Authentication);
            this.Dispatcher.Invoke(() =>
            {
                this.IconStatus.Visibility = Visibility.Visible;
            });
            if (result)
            {
                Uri uriCheckWorkOrder = new Uri(Helper.ServerAPI, $"Monitoring/CheckWorkOrder/{workOrder}");
                var resultSituation = APIClient.GetData<bool>(uriCheckWorkOrder.ToString(), Helper.Authentication);
                if (resultSituation)
                {
                    //Ordem de serviço existente e em aberto
                    this.Dispatcher.Invoke(() => 
                    {
                        this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                        this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
                        this.ButtonOpenWO.IsEnabled = true;
                    });
                    return true;
                }
                else
                {
                    this.Dispatcher.Invoke(() => 
                    {
                        this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("DarkOrange");
                        this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                    });
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CC0000");
                    this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                });

            }
            return false;
        }

        private void TextBoxWorkOrder_LostFocus(object sender, RoutedEventArgs e)
        {
            string workOrder = this.TextBoxWorkOrder.Text;
            Task.Run(() => 
            {
                StatusWO(workOrder);
            });
        }
    }
}
