using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Lógica interna para VisualizeProduct.xaml
    /// </summary>
    public partial class VisualizeProduct : Window
    {
        public ConsumptionProduct Product { get; private set; }
        public VisualizeProduct()
        {
            InitializeComponent();
        }
        public VisualizeProduct(ConsumptionProduct product)
        {
            InitializeComponent();
            this.Product = product;
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            UpdateMaterial dialog = new UpdateMaterial(Product);
            if (dialog.ShowDialog() == true)
            {
                //recebe os valores e atualiza lista
                if (dialog.Product.Photo != null)
                    this.ProductImage.Source = new BitmapImage(new Uri(Helper.Server, $"img/{dialog.Product.Photo}"));
                this.DescriptionTextBlock.Text = dialog.Product.Description;
                this.GroupLabel.Content = dialog.Product.Group;
                this.MininumStockLabel.Content = dialog.Product.MininumStock;
                this.MaximumStockLabel.Content = dialog.Product.MaximumStock;
                this.UnityLabel.Content = dialog.Product.Unity;
                this.LocalizationLabel.Content = dialog.Product.Localization;
            }
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja apagar este material?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            var product = ((FrameworkElement)sender).DataContext as Models.StockQuery;

            var result = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"generalproduct/remove/{product.ConsumptionProduct.Id}").ToString(), Helper.Authentication);
            MessageBox.Show(result.DeserializeJson<string>());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetNewImage();
        }
        private void GetNewImage()
        {
            string url = SearchUri("cabo espiral bege").ToString();
            var html = APIClient.GetData(url);
            //div class="mJxzWe"

            MessageBox.Show(html);
        }
        private Uri SearchUri(string query)
        {
            string url = @"https://www.google.com.br/search?tbm=isch&q=" + HttpUtility.UrlEncode(query);
            Uri uri = new Uri(url);
            return uri;
        }
    }
}
