using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using SCM2020___Client.Frames.Query;

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
            public double QuantityFuture { get => Quantity - QuantityAdded; }
            public double QuantityAdded { get; set; }
            public double Quantity { get; set; }
            public string Description { get; set; }
        }
        class PermanentProductDataGrid : ProductToOutput
        {
            public string Patrimony { get; set; }
            public string BtnContent { get; set; } = "Adicionar";
            public PermanentProductDataGrid()
            {

            }
            public PermanentProductDataGrid(SearchPermanentProduct searchPermanentProduct)
            {
                this.Code = searchPermanentProduct.ConsumptionProduct.Code;
                this.Description = searchPermanentProduct.ConsumptionProduct.Description;
                this.Id = searchPermanentProduct.InformationProduct;
                this.Patrimony = searchPermanentProduct.Patrimony;
                this.Quantity = searchPermanentProduct.ConsumptionProduct.Stock;
            }
        }
        class SearchPermanentProduct : ModelsLibraryCore.PermanentProduct
        {
            public ModelsLibraryCore.ConsumptionProduct ConsumptionProduct { get; set; }
            public SearchPermanentProduct(ModelsLibraryCore.PermanentProduct PermanentProduct)
            {
                int id = PermanentProduct.InformationProduct;
                ConsumptionProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{id}").ToString(), Helper.Authentication);
            }
        }
        public MaterialOutput()
        {
            InitializeComponent();
            Uri vendorUri = new Uri(Helper.Server, "vendor/");
            //var vendors = APIClient.GetData<List<Vendor>>(vendorUri.ToString());
            //var nameVendors = vendors.Select(x => x.Name).ToList();
            //this.VendorComboBox.ItemsSource = nameVendors;

            ProductToOutput ProductToOutput = new ProductToOutput()
            {
                Id = 1,
                Code = 1,
                Description = "TESTE",
                Quantity = 10
            };
            ProductToAddDataGrid.Items.Add(ProductToOutput);
            PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
            {
                Id = 1,
                Code = 2,
                Description = "TESTE2",
                Quantity = 12,
                Patrimony = "5621034",
            };
            PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid);
        }
        private void ConsumpterProductSearch()
        {
            string textBoxValue = string.Empty;
            this.TxtSearchConsumpterProduct.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtSearchConsumpterProduct.Text; }));

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
        private void PermanentProductSearch()
        {
            string textBoxValue = string.Empty;
            this.TxtPermanentProductSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtPermanentProductSearch.Text; }));
            
            Uri uriProductsSearch = new Uri(Helper.Server, $"PermanentProduct/patrimony/{textBoxValue}");

            this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Clear(); }));

            List<PermanentProduct> products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
            foreach (var product in products)
            {
                SearchPermanentProduct searchPermanentProduct = new SearchPermanentProduct(product);

            }
        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
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
                //int index = ProductToAddDataGrid.SelectedIndex;
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
            //SUPONDO QUE NÃO EXISTA UMA NOVA ORDEM DE SERVIÇO...
            DateTime dateTime = OSDatePicker.DisplayDate;
            if (OSDatePicker.DisplayDate == DateTime.Today)
            {
                dateTime = DateTime.Now;
            }
            var register = ApplicantTextBox.Text;
            var userId = APIClient.GetData<string>(new Uri(Helper.Server, $"User/UserId/{register}").ToString());
            //var userSCMId = APIClient.GetData<string>(new Uri(Helper.Server, $"User/UserId/{Helper.SCMRegistration}").ToString());

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...

            Monitoring monitoring = new Monitoring()
            {
                SCMEmployeeId = Helper.SCMId,
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

            foreach (var item in FinalConsumpterProductsAddedDataGrid.Items)
            {
                ConsumpterProductDataGrid outputProduct = item as ConsumpterProductDataGrid;
                //var code = outputProduct.Code;
                //var ConsumpterProduct = APIClient.GetData<ConsumptionProduct>(new Uri(Helper.Server, $"GeneralProduct/Code/{code}").ToString());

                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = materialOutput.MovingDate,
                    Quantity = outputProduct.Quantity,
                    ProductId = outputProduct.Id, //verificar se o ID é o mesmo do produto...
                    SCMEmployeeId = Helper.SCMId
                };
                materialOutput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (var item in FinalPermanentProductsAddedDataGrid.Items)
            {
                PermanentProductDataGrid outputPermanentProduct = item as PermanentProductDataGrid;
                AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                {
                    Date = materialOutput.MovingDate,
                    SCMEmployeeId = Helper.SCMId,
                    ProductId = outputPermanentProduct.Id
                };
                materialOutput.PermanentProducts.Add(auxiliarPermanent);
            }


            Task.Run(() => 
            {
                //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...

                var result1 = APIClient.PostData(new Uri(Helper.Server, "Monitoring/Add").ToString(), monitoring, Helper.Authentication);
                MessageBox.Show(result1);

                //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO

                var result2 = APIClient.PostData(new Uri(Helper.Server, "Output/Add").ToString(), materialOutput, Helper.Authentication);
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
        private void BtnAddRemovePermanent_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as PermanentProductDataGrid;
            if (product.BtnContent == "Adicionar")
            {
                this.FinalPermanentProductsAddedDataGrid.Items.Add(product);
                product.QuantityAdded += 1;
                product.BtnContent = "Remover";
            }
            else
            {
                product.BtnContent = "Adicionar";
                product.QuantityAdded -= 1;
                this.FinalPermanentProductsAddedDataGrid.Items.Remove(product);
            }
            this.PermanentProductToAddDataGrid.Items.Refresh();
            this.FinalPermanentProductsAddedDataGrid.Items.Refresh();
            this.PermanentProductToAddDataGrid.UnselectAll();
            this.FinalPermanentProductsAddedDataGrid.UnselectAll();
        }
        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            new Task(() => ConsumpterProductSearch()).Start();
        }
        private void TxtSearchConsumpterProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                new Task(() => ConsumpterProductSearch()).Start();
            }
        }
        private void PermanentProductSearchButton_Click(object sender, RoutedEventArgs e)
        {
            new Task(() => PermanentProductSearch()).Start();
        }
        private void TxtPermanentProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                new Task(() => PermanentProductSearch()).Start();
            }
        }
    }
}
