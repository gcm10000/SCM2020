﻿using ModelsLibraryCore.RequestingClient;
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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Product.xam
    /// </summary>
    public partial class ConsumpterProduct : UserControl
    {
        public ConsumpterProduct()
        {
            InitializeComponent();
        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                ModelsLibraryCore.ConsumptionProduct consumptionProduct = new ModelsLibraryCore.ConsumptionProduct()
                {
                    Code = int.Parse(CodeTextBox.Text),
                    Description = DescriptionTextBox.Text,
                    Group = int.Parse(GroupTextBox.Text),
                    Localization = LocalizationTextBox.Text,
                    MininumStock = double.Parse(MininumStockTextBox.Text),
                    MaximumStock = double.Parse(MaximumStockTextBox.Text),
                    NumberLocalization = uint.Parse(NumberLocalizationTextBox.Text),
                    Stock = double.Parse(StockTextBox.Text),
                    Unity = UnityTextBox.Text,
                    Photo = null
                };
                var result = APIClient.PostData(new Uri(Helper.Server, new Uri("generalProduct/Add/")).ToString(), consumptionProduct, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();
        }
    }
}