using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using WebAssemblyLibrary;

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
            this.VendorComboBox.SelectedIndex = 0;
        }
        private bool existsInput = false;
        private ModelsLibraryCore.MaterialInputByVendor previousInput;

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearch.Text;
            Task.Run(() => ConsumpterProductSearch(query));
        }
        private void ConsumpterProductSearch(string query)
        {
            if (query == string.Empty)
                return;
            this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Clear(); }));

            Uri uriProductsSearch = new Uri(Helper.Server, $"generalproduct/search/{query}");

            //Requisição de dados baseado na busca
            var products = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);

            foreach (var item in products)
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

        //private void Search()
        //{
        //    string textBoxValue = string.Empty;
        //    this.TxtSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtSearch.Text; }));

        //    //Se o campo está vazio, não irá realizar a busca
        //    if (textBoxValue == string.Empty)
        //    {
        //        return;
        //    }

        //    Uri uriProductsSearch = new Uri(Helper.Server, $"generalproduct/search/{textBoxValue}");

        //    this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Clear(); }));

        //    List<ConsumptionProduct> products = new List<ConsumptionProduct>();

        //    int index = -1;
        //    if (int.TryParse(textBoxValue, out _))
        //    {
        //        Uri uriProductsCode = new Uri(Helper.Server, $"generalproduct/code/{textBoxValue}");
        //        Task.Run(() => 
        //            {
        //                try
        //                {
        //                    var singleProduct = APIClient.GetData<ConsumptionProduct>(uriProductsCode.ToString());
        //                    products.Add(singleProduct);
        //                    index = products.FindIndex(x => x.Id == singleProduct.Id);
        //                }
        //                catch (System.Net.Http.HttpRequestException) { }
        //            });

        //    }

        //    var data = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
        //    products.AddRange(data);

        //    if (index > -1)
        //    {
        //        var myProduct = products[index];
        //        products[index] = products[0];
        //        products[0] = myProduct;
        //    }

        //    foreach (var item in products.ToList())
        //    {
        //        ConsumpterProductDataGrid productsToInput = new ConsumpterProductDataGrid()
        //        {
        //            Id = item.Id,
        //            Code = item.Code,
        //            Description = item.Description,
        //            Quantity = item.Stock
        //        };
        //        this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Add(productsToInput); }));
        //    }
        //}

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearch.Text;
                Task.Run(() => ConsumpterProductSearch(query));
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
                {
                    product.NewProduct = true;
                    ProductsAddedDataGrid.Items.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                        ProductsAddedDataGrid.Items.Remove(product);
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                        product.ProductChanged = true;

                    }
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
            if (!MovingDateDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Data invalida.", "Data inválida", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (existsInput)
                UpdateInput();
            else
                AddInput();

        }
        private void AddInput()
        {
            DateTime dateTime = (MovingDateDatePicker.DisplayDate == DateTime.Today) ? DateTime.Now : MovingDateDatePicker.SelectedDate.Value;
            MaterialInputByVendor materialInputByVendor = new MaterialInputByVendor();
            materialInputByVendor.Invoice = InvoiceTextBox.Text;
            materialInputByVendor.MovingDate = dateTime;
            materialInputByVendor.VendorId = VendorComboBox.SelectedIndex + 1;

            List<AuxiliarConsumption> p = new List<AuxiliarConsumption>();

            foreach (var item in ProductsAddedDataGrid.Items)
            {
                ConsumpterProductDataGrid product = item as ConsumpterProductDataGrid;
                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = DateTime.Now,
                    ProductId = product.Id,
                    Quantity = product.QuantityAdded,
                    SCMEmployeeId = Helper.NameIdentifier
                };
                p.Add(auxiliarConsumption);
            }
            materialInputByVendor.ConsumptionProducts = p;
            Task.Run(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.Server, "input/Add").ToString(), materialInputByVendor, Helper.Authentication);
                MessageBox.Show(result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void UpdateInput()
        {
            ModelsLibraryCore.MaterialInputByVendor input = this.previousInput;
            if (this.ProductsAddedDataGrid.Items.Count == 0)
            {
                //Pergunta se deseja apagar
                //Não há sentido de manter uma entrada por fornecedor com informações e sem materiais envolvidos
                MessageBoxResult resultDialog = MessageBox.Show("Você deseja apagar a entrada por fornecedor?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultDialog == MessageBoxResult.Yes)
                {
                    var result = APIClient.DeleteData(new Uri(Helper.Server, $"Input/Remove/{previousInput.Id}").ToString(), Helper.Authentication);
                    MessageBox.Show(result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            foreach (ConsumpterProductDataGrid item in this.ProductsAddedDataGrid.Items)
            {
                if (item.NewProduct)
                {
                    AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        Quantity = item.QuantityAdded,
                        SCMEmployeeId = Helper.NameIdentifier
                    };
                    item.NewProduct = false;
                    input.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }

            Task.Run(() => 
            {
                var result = APIClient.PostData(new Uri(Helper.Server, $"input/update/{input.Id}").ToString(), input, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        string previousInvoice = string.Empty;
        private void InvoiceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var invoice = InvoiceTextBox.Text;
            if (previousInvoice == invoice)
                return;
            Task.Run(() => RescueData(invoice));
            previousInvoice = invoice;
        }

        private void InvoiceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var invoice = InvoiceTextBox.Text;
            if (e.Key == Key.Enter)
            {
                if (previousInvoice == invoice)
                    return;
                Task.Run(() => RescueData(invoice));
                previousInvoice = invoice;
            }
        }

        private void RescueData(string invoice)
        {
            if (invoice == string.Empty)
                return;
            invoice = System.Uri.EscapeDataString(invoice);

            this.ProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductsAddedDataGrid.Items.Clear(); }));

            previousInput = null;
            try
            {
                previousInput = APIClient.GetData<ModelsLibraryCore.MaterialInputByVendor>(new Uri(Helper.Server, $"input/invoice/{invoice}").ToString(), Helper.Authentication);
                existsInput = true;
            }
            catch (HttpRequestException) //Entrada por fornecedor inexistente
            {
                //Limpar campos e permitir uso
                ClearData();
                InputData(true);
                existsInput = false;
                return;

            }
            //Bloquear combobox e data
            InputData(false);
            //Colocar informações
            this.VendorComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { VendorComboBox.SelectedIndex = previousInput.VendorId - 1; }));
            this.MovingDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { MovingDateDatePicker.SelectedDate = previousInput.MovingDate; }));
            //Preencher datagrid
            foreach (var item in previousInput.ConsumptionProducts)
            {
                ModelsLibraryCore.ConsumptionProduct information = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                ConsumpterProductDataGrid product = new ConsumpterProductDataGrid()
                {
                    Id = item.ProductId,
                    Code = information.Code,
                    Description = information.Description,
                    QuantityAdded = item.Quantity,
                    Quantity = information.Stock,
                    ConsumptionProduct = item
                };
                this.ProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductsAddedDataGrid.Items.Add(product); }));
            }
        }

        private void ClearData()
        {
            this.VendorComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { VendorComboBox.SelectedIndex = 0; }));
            this.MovingDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { MovingDateDatePicker.SelectedDate = DateTime.Now; }));
        }
        private void InputData(bool IsEnable)
        {
            this.VendorComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { VendorComboBox.IsEnabled = IsEnable; }));
            this.MovingDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { MovingDateDatePicker.IsEnabled = IsEnable; }));
        }
    }
}
