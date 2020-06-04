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
    public partial class MaterialOutput : UserControl
    {
        class ProductToOutput
        {
            //public string Image { get; set; }
            public int Id { get; set; }
            public int Code { get; set; }
            public double QuantityFuture { get => Quantity + QuantityAdded; }
            public double QuantityAdded { get; set; }
            public double Quantity { get; set; }
            public string Description { get; set; }
        }


        public MaterialOutput()
        {
            InitializeComponent();
            Uri vendorUri = new Uri(Helper.Server, "vendor/");
            //var vendors = APIClient.GetData<List<Vendor>>(vendorUri.ToString());
            //var nameVendors = vendors.Select(x => x.Name).ToList();
            //this.VendorComboBox.ItemsSource = nameVendors;

            //ProductToOutput ProductToOutput = new ProductToOutput()
            //{
            //    Id = 1,
            //    Code = 1,
            //    Description = "TESTE",
            //    Quantity = 10
            //};
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
            //ProductToAddDataGrid.Items.Add(ProductToOutput);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            new Task(() => Search()).Start();
        }
        private void Search()
        {
            string textBoxValue = string.Empty;
            this.TxtSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtSearch.Text; }));

            Uri uriProductsSearch = new Uri(Helper.Server, $"generalproduct/search/{textBoxValue}");

            this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Clear(); }));

            List<ConsumptionProduct> products = new List<ConsumptionProduct>();

            int index = -1;
            if (int.TryParse(textBoxValue, out _))
            {
                Uri uriProductsCode = new Uri(Helper.Server, $"generalproduct/code/{textBoxValue}");
                new Task(() =>
                {
                    var singleProduct = APIClient.GetData<ConsumptionProduct>(uriProductsCode.ToString());
                    products.Add(singleProduct);
                    index = products.FindIndex(x => x.Id == singleProduct.Id);

                }).Start();

            }

            var data = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString());
            products.AddRange(data);

            if (index > -1)
            {
                var myProduct = products[index];
                products[index] = products[0];
                products[0] = myProduct;
            }

            foreach (var item in products.ToList())
            {
                ProductToOutput productsToInput = new ProductToOutput()
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
                new Task(() => Search()).Start();
            }
        }

        private void VendorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(this.VendorComboBox.ActualWidth.ToString());
        }

        private void BtnInformation_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = false;
            this.ButtonPermanentProducts.IsHitTestVisible = true;
            this.ButtonFinish.IsHitTestVisible = true;

            this.InfoScrollViewer.Visibility = Visibility.Visible;
            this.InfoDockPanel.Visibility = Visibility.Visible;
            this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
            this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }
        private void ButtonPermanentProducts_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = true;
            this.ButtonPermanentProducts.IsHitTestVisible = false;
            this.ButtonFinish.IsHitTestVisible = true;

            InfoScrollViewer.Visibility = Visibility.Collapsed;
            InfoDockPanel.Visibility = Visibility.Collapsed;
            PermanentDockPanel.Visibility = Visibility.Visible;
        }
        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonInformation.IsHitTestVisible = true;
            this.ButtonPermanentProducts.IsHitTestVisible = true;
            this.ButtonFinish.IsHitTestVisible = false;

            this.InfoScrollViewer.Visibility = Visibility.Collapsed;
            this.InfoDockPanel.Visibility = Visibility.Collapsed;
            this.FinalProductsDockPanel.Visibility = Visibility.Visible;
            this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as ProductToOutput;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                int index = ProductToAddDataGrid.SelectedIndex;
                ProductToAddDataGrid.Items.Refresh();
                FinalConsumpterProductsAddedDataGrid.Items.Refresh();
                if (!FinalConsumpterProductsAddedDataGrid.Items.Contains(product))
                    FinalConsumpterProductsAddedDataGrid.Items.Add(product);
                else
                {
                    if (dialog.QuantityAdded == 0)
                        FinalConsumpterProductsAddedDataGrid.Items.Remove(product);
                    else
                        product.QuantityAdded = dialog.QuantityAdded;
                }
                ProductToAddDataGrid.UnselectAll();
                FinalConsumpterProductsAddedDataGrid.UnselectAll();
            }
        }
        private void ProductToAddDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            var currentRowIndex = ProductToAddDataGrid.Items.IndexOf(ProductToAddDataGrid.CurrentItem);
            MessageBox.Show(currentRowIndex.ToString());
        }
        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            //AQUI SE ADICIONA UM NOVO MONITORAMENTO E UMA NOVA SAÍDA

            DateTime dateTime = OSDatePicker.DisplayDate;
            if (OSDatePicker.DisplayDate == DateTime.Today)
            {
                dateTime = DateTime.Now;
            }
            var register = ApplicantTextBox.Text;
            var userId = APIClient.GetData<string>(new Uri(Helper.Server, new Uri($"UserId/{register}")).ToString());

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...

            Monitoring monitoring = new Monitoring()
            {
                SCMEmployeeId = Helper.SCMEmployee,
                Situation = false,
                ClosingDate = null,
                EmployeeId = userId,
                RequestingSector = 0,
                MovingDate = dateTime,
                Work_Order = OSTextBox.Text
            };

            //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO

            var materialOutput = new ModelsLibraryCore.MaterialOutput
            {
                WorkOrder = OSTextBox.Text,
                MovingDate = dateTime,
                ServiceLocation = ServiceLocalizationTextBox.Text,
                
            };

            foreach (var item in ProductToAddDataGrid.Items)
            {
                ProductDataGrid outputProduct = item as ProductDataGrid;
                var code = outputProduct.Code;
                var ConsumpterProduct = APIClient.GetData<ConsumptionProduct>(new Uri(Helper.Server, new Uri($"GeneralProduct/Code/{code}")).ToString());

                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = materialOutput.MovingDate,
                    Quantity = outputProduct.Quantity,
                    ProductId = ConsumpterProduct.Id,
                    SCMRegistration = Helper.SCMRegistration
                };
            }


            Task.Run(() => 
            {
                //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...

                var result1 = APIClient.PostData(new Uri(Helper.Server, "Monitoring/Add").ToString(), null, Helper.Authentication);
                MessageBox.Show(result1);

                //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO

                var result2 = APIClient.PostData(new Uri(Helper.Server, "Output/Add").ToString(), null, Helper.Authentication);
                MessageBox.Show(result2);
            }).Start();

        }

        private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Visible;
            this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Collapsed;
        }

        private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        {
            this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Visible;
            this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Collapsed;
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
