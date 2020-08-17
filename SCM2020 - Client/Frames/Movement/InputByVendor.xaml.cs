using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Interação lógica para InputByVendor.xaml
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        public InputByVendor()
        {
            InitializeComponent();
            Uri vendorUri = new Uri(Helper.Server, "vendor/");
            var vendors = APIClient.GetData<List<Vendor>>(vendorUri.ToString());
            var nameVendors = vendors.Select(x => x.Name).ToList();
            this.VendorComboBox.ItemsSource = nameVendors;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(Search);
        }
        private void Search()
        {
            string textBoxValue = string.Empty;
            this.TxtSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtSearch.Text; }));

            //Se o campo está vazio, não irá realizar a busca
            if (textBoxValue == string.Empty)
            {
                return;
            }

            Uri uriProductsSearch = new Uri(Helper.Server, $"generalproduct/search/{textBoxValue}");

            this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Clear(); }));

            List<ConsumptionProduct> products = new List<ConsumptionProduct>();

            int index = -1;
            if (int.TryParse(textBoxValue, out _))
            {
                Uri uriProductsCode = new Uri(Helper.Server, $"generalproduct/code/{textBoxValue}");
                Task.Run(() => 
                    {
                        try
                        {
                            var singleProduct = APIClient.GetData<ConsumptionProduct>(uriProductsCode.ToString());
                            products.Add(singleProduct);
                            index = products.FindIndex(x => x.Id == singleProduct.Id);
                        }
                        catch (System.Net.Http.HttpRequestException) { }
                    });

            }

            var data = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
            products.AddRange(data);

            if (index > -1)
            {
                var myProduct = products[index];
                products[index] = products[0];
                products[0] = myProduct;
            }

            foreach (var item in products.ToList())
            {
                ConsumpterProductDataGrid productsToInput = new ConsumpterProductDataGrid()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    Quantity = item.Stock
                };
                this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Add(productsToInput); }));
            }
        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Task.Run(Search);
            }
        }

        private void VendorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(this.VendorComboBox.ActualWidth.ToString());
        }

        private void BtnInformation_Click(object sender, RoutedEventArgs e)
        {
            ButtonInformation.IsHitTestVisible = false;
            ButtonProducts.IsHitTestVisible = true;
            InfoScrollViewer.Visibility = Visibility.Visible;
            InfoDockPanel.Visibility = Visibility.Visible;
            ProductsDockPanel.Visibility = Visibility.Collapsed;
        }
        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            ButtonInformation.IsHitTestVisible = true;
            ButtonProducts.IsHitTestVisible = false;

            InfoScrollViewer.Visibility = Visibility.Collapsed;
            InfoDockPanel.Visibility = Visibility.Collapsed;
            ProductsDockPanel.Visibility = Visibility.Visible;

        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as ConsumpterProductDataGrid;
            var dialog = new DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                int index = ProductToAddDataGrid.SelectedIndex;
                ProductToAddDataGrid.Items.Refresh();
                ProductsAddedDataGrid.Items.Refresh();
                if (!ProductsAddedDataGrid.Items.Contains(product))
                    ProductsAddedDataGrid.Items.Add(product);
                else
                {
                    if (dialog.QuantityAdded == 0)
                        ProductsAddedDataGrid.Items.Remove(product);
                    else
                        product.QuantityAdded = dialog.QuantityAdded;
                }
                ProductToAddDataGrid.UnselectAll();
                ProductsAddedDataGrid.UnselectAll();
            }
        }
        private void ProductToAddDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            var currentRowIndex = ProductToAddDataGrid.Items.IndexOf(ProductToAddDataGrid.CurrentItem);
            MessageBox.Show(currentRowIndex.ToString());
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = (MovingDateDatePicker.DisplayDate == DateTime.Today) ? DateTime.Now : MovingDateDatePicker.DisplayDate;
            MaterialInputByVendor materialInputByVendor = new MaterialInputByVendor();
            materialInputByVendor.Invoice = InvoiceTextBox.Text;
            materialInputByVendor.MovingDate = dateTime;

            List<AuxiliarConsumption> p = new List<AuxiliarConsumption>();

            foreach (var item in ProductsAddedDataGrid.Items)
            {
                ConsumpterProductDataGrid product = item as ConsumpterProductDataGrid;
                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                { 
                    Date = materialInputByVendor.MovingDate,
                    ProductId = product.Id,
                    Quantity = product.QuantityAdded
                };
                p.Add(auxiliarConsumption);
            }
            materialInputByVendor.AuxiliarConsumptions = p;
            Task.Run(() => 
            {
                var result = APIClient.PostData(new Uri(Helper.Server, "input/Add").ToString(), materialInputByVendor, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }
}
