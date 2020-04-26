using ModelsLibraryCore;
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

namespace SCM2020___Client.Frames.Movement
{
    /// <summary>
    /// Interação lógica para Devolution.xam
    /// </summary>
    public partial class Devolution : UserControl
    {
        public Devolution()
        {
            InitializeComponent();
        }

        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            new Task(() => CheckOS()).Start();
        }

        private void SearchOSButton_Click(object sender, RoutedEventArgs e)
        {
            //new Task(() => CheckOS()).Start();
            this.ButtonInformation.IsHitTestVisible = false;
            this.ButtonPermanentProducts.IsHitTestVisible = true;
            this.ButtonFinish.IsHitTestVisible = true;

            this.InfoScrollViewer.Visibility = Visibility.Visible;
            this.InfoDockPanel.Visibility = Visibility.Visible;
            this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
            this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }
        private void CheckOS()
        {
            string workorder = OSTextBox.Text;
            var uriRequest = new Uri(Helper.Server, new Uri($"monitoring/WorkOrder/{workorder}"));
            var resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);
            if (resultMonitoring.Situation == false)
            {
                this.ButtonInformation.IsHitTestVisible = false;
                this.ButtonPermanentProducts.IsHitTestVisible = true;
                this.ButtonFinish.IsHitTestVisible = true;

                this.InfoScrollViewer.Visibility = Visibility.Visible;
                this.InfoDockPanel.Visibility = Visibility.Visible;
                this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
                this.PermanentDockPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonInformation_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = false;
            this.ButtonPermanentProducts.IsHitTestVisible = true;
            this.ButtonFinish.IsHitTestVisible = true;

            this.InfoScrollViewer.Visibility = Visibility.Visible;
            this.InfoDockPanel.Visibility = Visibility.Visible;
            this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
            this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }
        private void ButtonPermanentProducts_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = true;
            this.ButtonPermanentProducts.IsHitTestVisible = false;
            this.ButtonFinish.IsHitTestVisible = true;

            InfoScrollViewer.Visibility = Visibility.Collapsed;
            InfoDockPanel.Visibility = Visibility.Collapsed;
            PermanentDockPanel.Visibility = Visibility.Visible;
        }
        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = true;
            this.ButtonPermanentProducts.IsHitTestVisible = true;
            this.ButtonFinish.IsHitTestVisible = false;

            this.InfoScrollViewer.Visibility = Visibility.Collapsed;
            this.InfoDockPanel.Visibility = Visibility.Collapsed;
            this.FinalProductsDockPanel.Visibility = Visibility.Visible;
            this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VendorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
