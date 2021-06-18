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
            this.DatePickerClosureOSDate.SelectedDate = DateTime.Now;
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

        private void TextBoxWorkOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Captura a ordem de serviço escrita pelo usuário
                string workOrder = TextBoxWorkOrder.Text;
                DateTime dateTime = (DatePickerClosureOSDate.DisplayDate == DateTime.Today) ? DateTime.Now : DatePickerClosureOSDate.DisplayDate;
                Task.Run(() =>
                {
                    if (StatusWO(workOrder))
                    {
                        ClosureWO(workOrder, dateTime.Year, dateTime.Month, dateTime.Day);
                    }
                });
            }
        }

        private bool StatusWO(string workOrder)
        {
            if (workOrder == string.Empty)
                return false;
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
                if (!resultSituation)
                {
                    //Ordem de serviço existente e em aberto
                    this.Dispatcher.Invoke(() =>
                    {
                        this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                        this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
                        this.IconStatus.ToolTip = "Ordem de serviço aberta.";
                        this.ButtonFinish.IsEnabled = true;
                    });
                    return true;
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("DarkOrange");
                        this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                        this.IconStatus.ToolTip = "Ordem de serviço fechada.";
                        this.ButtonFinish.IsEnabled = false;
                    });
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.IconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CC0000");
                    this.IconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                    this.IconStatus.ToolTip = "Ordem de serviço inexistente.";
                    this.ButtonFinish.IsEnabled = false;
                });

            }
            return false;
        }

        private void DatePickerClosureOSDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Captura a ordem de serviço escrita pelo usuário
                string workOrder = TextBoxWorkOrder.Text;
                DateTime dateTime = (DatePickerClosureOSDate.DisplayDate == DateTime.Today) ? DateTime.Now : DatePickerClosureOSDate.DisplayDate;
                Task.Run(() =>
                {
                    if (StatusWO(workOrder))
                    {
                        ClosureWO(workOrder, dateTime.Year, dateTime.Month, dateTime.Day);
                    }
                });
            }
        }

        private void TextBoxWorkOrder_KeyUp(object sender, KeyEventArgs e)
        {
            string workOrder = this.TextBoxWorkOrder.Text;
            Task.Run(() =>
            {
                StatusWO(workOrder);
            });
        }
    }
}
