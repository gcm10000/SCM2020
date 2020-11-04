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

        }
    }
}
