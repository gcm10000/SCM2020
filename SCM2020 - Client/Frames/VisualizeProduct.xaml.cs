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
        public bool RemovedProduct { get; private set; } = false;
        public VisualizeProduct()
        {
            InitializeComponent();

        }
        public VisualizeProduct(ConsumptionProduct product)
        {
            InitializeComponent();
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.Product = product;
            FillUI();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            UpdateMaterial dialog = new UpdateMaterial(Product);
            if (dialog.ShowDialog() == true)
            {
                Product = dialog.Product;
                FillUI();
                this.DialogResult = true;
            }
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja apagar este material?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            var result = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"generalproduct/remove/{Product.Id}").ToString(), Helper.Authentication);
            this.DialogResult = true;
            RemovedProduct = true;
            MessageBox.Show(result.DeserializeJson<string>());
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void FillUI()
        {
            if (Product.Photo != null)
                this.ProductImage.Source = new BitmapImage(new Uri(Helper.Server, Product.Photo));
            else
                this.ProductImage.Source = Helper.DataImageNotAvaliable.LoadImage();
            this.DescriptionTextBlock.Text = $"{Product.Description} ({Product.Code})";
            this.GroupLabel.Content = APIClient.GetData<ModelsLibraryCore.Group>(new Uri(Helper.ServerAPI, $"group/{Product.Group}").ToString(), Helper.Authentication).GroupName;
            this.MininumStockLabel.Content = $"Quantidade Mínima: {Product.MininumStock}";
            this.StockLabel.Content = $"Quantidade em Estoque: {Product.Stock}";
            this.MaximumStockLabel.Content = $"Quantidade Máxima: {Product.MaximumStock}";
            this.UnityLabel.Content = $"Unidade: {Product.Unity}";
            if ((Product.Localization.Trim() != string.Empty) || (Product.Localization == null))
                this.LocalizationLabel.Content = $"Localização: {Product.Localization}";

        }

        private void ProductImage_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;
        }

        private void ProductImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;
        }

        private void PackIconForward_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;

        }

        private void PackIconForward_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;
        }

        private void PackIconBack_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;

        }

        private void PackIconBack_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void GridImageLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;
            this.PackIconBack.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A0000000"));
        }

        private void GridImageLeft_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;
            this.PackIconBack.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#80000000"));
        }

        private void GridImageRight_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;
            this.PackIconForward.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A0000000"));
        }

        private void GridImageRight_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;
            this.PackIconForward.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#80000000"));
        }

        private void GridCount_MouseEnter(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Visible;
            this.PackIconForward.Visibility = Visibility.Visible;
            this.GridCount.Visibility = Visibility.Visible;
        }

        private void GridCount_MouseLeave(object sender, MouseEventArgs e)
        {
            this.PackIconBack.Visibility = Visibility.Hidden;
            this.PackIconForward.Visibility = Visibility.Hidden;
            this.GridCount.Visibility = Visibility.Hidden;
        }
    }
}
