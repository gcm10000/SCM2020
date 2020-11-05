﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Lógica interna para UpdateMaterial.xaml
    /// </summary>
    public partial class UpdateMaterial : Window
    {
        public ConsumptionProduct Product { get; private set; }
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


            Uri groupUri = new Uri(Helper.Server, $"group/");
            var groups = APIClient.GetData<List<ModelsLibraryCore.Group>>(groupUri.ToString());
            var nameGroups = groups.Select(x => x.GroupName).ToList();
            this.GroupComboBox.ItemsSource = nameGroups;
            this.GroupComboBox.SelectedIndex = this.GroupComboBox.Items.IndexOf(groups.Single(x => x.Id == product.Group).GroupName);
            this.LocalizationTextBox.Text = product.Localization;
            this.MininumStockTextBox.Text = product.MininumStock.ToString();
            this.MaximumStockTextBox.Text = product.MaximumStock.ToString();
            this.NumberLocalizationTextBox.Text = product.NumberLocalization.ToString();
            this.StockTextBox.Text = product.Stock.ToString();
            this.UnityTextBox.Text = product.Unity;

        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            Product = new ConsumptionProduct()
            {
                Id = Product.Id,
                Code = int.Parse(CodeTextBox.Text),
                Description = DescriptionTextBox.Text,
                Localization = LocalizationTextBox.Text,
                MininumStock = double.Parse(MininumStockTextBox.Text),
                MaximumStock = double.Parse(MaximumStockTextBox.Text),
                NumberLocalization = uint.Parse(NumberLocalizationTextBox.Text),
                Stock = double.Parse(StockTextBox.Text),
                Unity = UnityTextBox.Text,
                Group = (GroupComboBox.SelectedIndex + 1),
                Photo = null
            };
            Task.Run(() => 
            {
                var result = APIClient.PostData(new Uri(Helper.Server, $"generalproduct/update/{Product.Id}"), Product, Helper.Authentication);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Dispatcher.Invoke(new Action(() => { this.DialogResult = true; this.Close(); }));
                }
            });
        }
    }
}