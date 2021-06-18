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
        private Window window = null;
        public ModelsLibraryCore.PermanentProduct Permanent_Product = null;
        public ModelsLibraryCore.ConsumptionProduct Consumption_Product = null;
        public PermanentProduct()
        {
            InitializeComponent();
            InitializeComboBox();
        }
        public PermanentProduct(Window window)
        {
            InitializeComponent();
            InitializeComboBox();
            this.window = window;
        }
        private void InitializeComboBox()
        {
            StatusComboBox.Items.Add("Novo");
            StatusComboBox.Items.Add("Recondicionado");
            StatusComboBox.Items.Add("Descartado");
        }

        private void BtnPermamentProduct_Click(object sender, RoutedEventArgs e)
        {
            ModelsLibraryCore.PermanentProduct permanentProduct = new ModelsLibraryCore.PermanentProduct()
            {
                DateAdd = DateTime.Now,
                Status = (ModelsLibraryCore.Status)StatusComboBox.SelectedIndex,
                InformationProduct = Consumption_Product.Id,
                Patrimony = PatrimonyTextBox.Text
            };
            if (window == null)
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"permanentproduct/add").ToString(), permanentProduct, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Permanent_Product = permanentProduct;
                window.Close();
            }
        }

        private void CodeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (CodeTextBox.Text.Trim() == "")
                return;
            try
            {
                var result = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"generalproduct/CheckCode/{CodeTextBox.Text.Trim()}").ToString(), Helper.Authentication);
                if (result)
                {
                    Consumption_Product = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/code/{CodeTextBox.Text.Trim()}").ToString(), Helper.Authentication);
                    DescriptionTextBox.Text = Consumption_Product.Description;
                }
                else
                {
                    DescriptionTextBox.Text = string.Empty;
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Erro:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
