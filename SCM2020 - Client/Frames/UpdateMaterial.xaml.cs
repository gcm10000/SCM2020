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
using System.Windows.Shapes;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Lógica interna para UpdateMaterial.xaml
    /// </summary>
    public partial class UpdateMaterial : Window
    {
        public ConsumptionProduct Product { get; }
        public UpdateMaterial()
        {
            InitializeComponent();
        }

        public UpdateMaterial(ConsumptionProduct product)
        {
            InitializeComponent();
            Product = product;
            this.CodeTextBox.Text = product.Code.ToString();
            this.DescriptionTextBox.Text = product.Description;
            //this.GroupComboBox
            this.LocalizationTextBox.Text = product.Localization;
            this.MininumStockTextBox.Text = product.MininumStock.ToString();
            this.MaximumStockTextBox.Text = product.MaximumStock.ToString();
            this.NumberLocalizationTextBox.Text = product.NumberLocalization.ToString();
            this.StockTextBox.Text = product.Stock.ToString();
            this.UnityTextBox.Text = product.Unity;

        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            ConsumptionProduct consumptionProduct = new ConsumptionProduct()
            {
                Id = Product.Id,
                Code = int.Parse(CodeTextBox.Text),
                Description = DescriptionTextBox.Text,
                Localization = LocalizationTextBox.Text,
                MininumStock = int.Parse(MininumStockTextBox.Text),
                MaximumStock = int.Parse(MaximumStockTextBox.Text),
                NumberLocalization = uint.Parse(NumberLocalizationTextBox.Text),
                Stock = double.Parse(StockTextBox.Text),
                Unity = UnityTextBox.Text,
                //Group
            };
            var result = APIClient.PostData(new Uri(Helper.Server, $"generalproduct/update/{Product.Id}"), consumptionProduct, Helper.Authentication);
        }
    }
}
