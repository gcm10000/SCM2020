using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para PermanentProduct.xam
    /// </summary>
    public partial class PermanentProduct : UserControl
    {
        public PermanentProduct()
        {
            InitializeComponent();
            StatusComboBox.Items.Add("Novo");
            StatusComboBox.Items.Add("Recondicionado");
            StatusComboBox.Items.Add("Descartado");
        }
        ModelsLibraryCore.ConsumptionProduct product;

        private void BtnPermamentProduct_Click(object sender, RoutedEventArgs e)
        {
            ModelsLibraryCore.PermanentProduct permanentProduct = new ModelsLibraryCore.PermanentProduct()
            {
                DateAdd = DateTime.Now,
                Status = (ModelsLibraryCore.Status)StatusComboBox.SelectedIndex,
                InformationProduct = product.Id,
                Patrimony = PatrimonyTextBox.Text
            };
            var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"permanentproduct/add").ToString(), permanentProduct, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CodeTextBox.Text.Trim() == "")
                return;
            try
            {
                product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/code/{CodeTextBox.Text.Trim()}").ToString(), Helper.Authentication);
                DescriptionTextBox.Text = product.Description;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Produto inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
