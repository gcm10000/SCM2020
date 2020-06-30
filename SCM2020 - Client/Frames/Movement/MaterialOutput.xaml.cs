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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;
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
            public bool NewProduct { get; set; }
            public bool ProductChanged { get; set; }
            public ModelsLibraryCore.AuxiliarConsumption ConsumptionProduct { get; set; }
        }
        class PermanentProductDataGrid : ProductToOutput
        {
            public string Patrimony { get; set; }
            public string BtnContent { get  => (QuantityAdded == 1) ?  "Remover": "Adicionar"; }
            public PermanentProductDataGrid()
            {

            }
            public PermanentProductDataGrid(SearchPermanentProduct searchPermanentProduct)
            {
                this.Code = searchPermanentProduct.ConsumptionProduct.Code;
                this.Description = searchPermanentProduct.ConsumptionProduct.Description;
                this.Id = searchPermanentProduct.Id;
                this.Patrimony = searchPermanentProduct.Patrimony;
                this.Quantity = searchPermanentProduct.ConsumptionProduct.Stock;
            }
        }
        class SearchPermanentProduct : ModelsLibraryCore.PermanentProduct
        {
            public ModelsLibraryCore.ConsumptionProduct ConsumptionProduct { get; set; }
            public SearchPermanentProduct(ModelsLibraryCore.PermanentProduct PermanentProduct)
            {
                this.Id = PermanentProduct.Id;
                this.InformationProduct = PermanentProduct.InformationProduct;
                this.Patrimony = PermanentProduct.Patrimony;
                ConsumptionProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{this.InformationProduct}").ToString(), Helper.Authentication);
            }
        }
        public Monitoring PrincipalMonitoring { get; set; }
        public InfoUser InfoUser { get; set; }
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
            //PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
            //{
            //    Id = 1,
            //    Code = 2,
            //    Description = "TESTE2",
            //    Quantity = 12,
            //    Patrimony = "5621034",
            //};
            //PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid);
        }
        private bool previousOutputExists = false;
        private ModelsLibraryCore.MaterialOutput previousMaterialOutput = null;
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
                ProductToOutput productsToOutput = new ProductToOutput()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    Quantity = item.Stock,
                };
                this.ProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductToAddDataGrid.Items.Add(productsToOutput); }));

            }
        }
        private void PermanentProductSearch()
        {
            string textBoxValue = string.Empty;
            this.TxtPermanentProductSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { textBoxValue = TxtPermanentProductSearch.Text; }));
            
            Uri uriProductsSearch = new Uri(Helper.Server, $"PermanentProduct/search/{textBoxValue}");

            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { PermanentProductToAddDataGrid.Items.Clear(); }));

            List<PermanentProduct> products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
            foreach (var product in products)
            {
                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid(new SearchPermanentProduct(product));
                this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid); }));
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
        List<ProductToOutput> FinalConsumpterProductsAdded = new List<ProductToOutput>();

        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = ((FrameworkElement)sender);
            //BtnAdded
            var product = ((FrameworkElement)sender).DataContext as ProductToOutput;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                //int index = ProductToAddDataGrid.SelectedIndex;
                ProductToAddDataGrid.Items.Refresh();
                FinalConsumpterProductsAddedDataGrid.Items.Refresh();
                if (!FinalConsumpterProductsAddedDataGrid.Items.Contains(product))
                {
                    product.NewProduct = button.Name == "BtnAdd";
                    FinalConsumpterProductsAddedDataGrid.Items.Add(product);
                    FinalConsumpterProductsAdded.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                    {
                        //this.previousMaterialOutput.ConsumptionProducts.Remove(product.ConsumptionProduct);
                        FinalConsumpterProductsAddedDataGrid.Items.Remove(product);
                        //FinalConsumpterProductsAdded.Remove(product);
                    }
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                    }
                    product.ProductChanged = true;
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
            if (previousOutputExists)
                UpdateOutput();
            else
                AddOutput();
        }
        private void AddOutput()
        {
            //AQUI SE ADICIONA UM NOVO MONITORAMENTO E UMA NOVA SAÍDA
            //SUPONDO QUE NÃO EXISTA UMA NOVA ORDEM DE SERVIÇO...
            DateTime dateTime = (OSDatePicker.DisplayDate == DateTime.Today) ? (DateTime.Now) : OSDatePicker.DisplayDate;

            var register = ApplicantTextBox.Text;
            var userId = APIClient.GetData<string>(new Uri(Helper.Server, $"User/UserId/{register}").ToString());
            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...

            Monitoring monitoring = new Monitoring()
            {
                SCMEmployeeId = Helper.NameIdentifier,
                Situation = false,
                ClosingDate = null,
                EmployeeId = userId,
                RequestingSector = Helper.CurrentSector.Id,
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

            if (FinalConsumpterProductsAddedDataGrid.Items.Count > 0)
                materialOutput.ConsumptionProducts = new List<AuxiliarConsumption>();
            if (FinalPermanentProductsAddedDataGrid.Items.Count > 0)
                materialOutput.PermanentProducts = new List<AuxiliarPermanent>();

            foreach (var item in FinalConsumpterProductsAddedDataGrid.Items)
            {
                ProductToOutput outputProduct = item as ProductToOutput;
                //var code = outputProduct.Code;
                //var ConsumpterProduct = APIClient.GetData<ConsumptionProduct>(new Uri(Helper.Server, $"GeneralProduct/Code/{code}").ToString());

                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = materialOutput.MovingDate,
                    Quantity = outputProduct.QuantityAdded,
                    ProductId = outputProduct.Id, //verificar se o ID é o mesmo do produto...
                    SCMEmployeeId = Helper.NameIdentifier
                };
                materialOutput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (var item in FinalPermanentProductsAddedDataGrid.Items)
            {
                PermanentProductDataGrid outputPermanentProduct = item as PermanentProductDataGrid;
                AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                {
                    Date = materialOutput.MovingDate,
                    SCMEmployeeId = Helper.NameIdentifier,
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

                PrincipalMonitoring = monitoring;
            }).Wait();
        }
        private void UpdateOutput()
        {
            ModelsLibraryCore.MaterialOutput materialOutput = this.previousMaterialOutput;
            if ((FinalConsumpterProductsAddedDataGrid.Items.Count > 0) && (materialOutput.ConsumptionProducts == null))
            {
                materialOutput.ConsumptionProducts = new List<AuxiliarConsumption>();
            }
            if ((FinalPermanentProductsAddedDataGrid.Items.Count > 0) && (materialOutput.PermanentProducts == null))
            {
                materialOutput.PermanentProducts = new List<AuxiliarPermanent>();
            }
            var listProduct = materialOutput.ConsumptionProducts.ToList();
            foreach (ProductToOutput item in FinalConsumpterProductsAdded)
            {
                if (item.NewProduct)
                {
                    AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        Quantity = item.QuantityAdded,
                        SCMEmployeeId = Helper.NameIdentifier,
                    };
                    item.NewProduct = false;
                    materialOutput.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    listProduct[FinalConsumpterProductsAdded.IndexOf(item)].Quantity =
                        item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
            {
                //MEXER
                if (!materialOutput.PermanentProducts.Any(x => x.ProductId == item.Id))
                {
                    AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        SCMEmployeeId = Helper.NameIdentifier
                    };
                    materialOutput.PermanentProducts.Add(auxiliarPermanent);
                }
            }
            var result = APIClient.PostData(new Uri(Helper.Server, $"output/Update/{materialOutput.Id}").ToString(), materialOutput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
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
                //product.BtnContent = "Remover";
            }
            else
            {
                //product.BtnContent = "Adicionar";
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

        private void OSTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var workOrder = OSTextBox.Text;
            if (previousOS == workOrder)
                return;
            else
                previousOS = workOrder;
            new Task(() => RescueData(workOrder)).Start();
        }
        string previousOS = string.Empty;
        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var workOrder = OSTextBox.Text;
            if (previousOS == workOrder)
                return;
            else
                previousOS = workOrder;
            if (e.Key == Key.Enter)
            {
                new Task(() => RescueData(workOrder)).Start();
            }
        }
        private void RescueData(string workOrder)
        {
            /*
             * QUANDO HOUVER ALGUMA SAÍDA ANTERIOR, NÃO SE USARÁ MAIS O MÉTODO /ADD/ DO SERVIDOR. SERÁ UTILIZADO O MÉTODO /UPDATE/ JUSTAMENTE PORQUE JÁ EXISTE UM UMA SAÍDA.
             * É UMA SAÍDA E UMA ENTRADA (DEVOLUÇÃO) PARA CADA MONITORAMENTO.
             * MONITORING -> MATERIALOUTPUT
             * MONITORING -> MATERIALINPUT
             */
            if (workOrder == string.Empty)
                return;
            try
            {
                workOrder = System.Uri.EscapeDataString(workOrder);
                //Check monitoring
                Monitoring monitoring = APIClient.GetData<Monitoring>(new Uri(Helper.Server, $"Monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                var userId = monitoring.EmployeeId;
                var result = APIClient.GetData<string>(new Uri(Helper.Server, $"User/RegisterId/{userId}").ToString(), Helper.Authentication);
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
                
                PrincipalMonitoring = monitoring;
                if (monitoring.Situation) //WORKORDER IS CLOSED.
                {
                    MessageBox.Show("Ordem de serviço fechada.", "Informação.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; //DISABLE FILLING DATA.
                }
                //ID -> MATRÍCULA
                var materialOutput = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.Server, $"Output/workOrder/{workOrder}").ToString(), Helper.Authentication);
                previousOutputExists = true;
                this.previousMaterialOutput = materialOutput;
                this.OSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSDatePicker.DisplayDate = monitoring.MovingDate; }));
                this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = materialOutput.ServiceLocation; }));

                this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicantTextBox.Text = InfoUser.Register; }));

                this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { FinalConsumpterProductsAddedDataGrid.Items.Clear(); }));
                this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { FinalPermanentProductsAddedDataGrid.Items.Clear(); }));
                foreach (var item in materialOutput.ConsumptionProducts)
                {
                    var consumpterProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    ProductToOutput productToOutput = new ProductToOutput()
                    {
                        Id = item.ProductId,
                        QuantityAdded = item.Quantity,
                        Description = consumpterProduct.Description,
                        Code = consumpterProduct.Code,
                        Quantity = consumpterProduct.Stock,
                        ConsumptionProduct = item
                    };
                    this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action (() => { FinalConsumpterProductsAddedDataGrid.Items.Add(productToOutput); }));
                    FinalConsumpterProductsAdded.Add(productToOutput);
                }
                foreach (var item in materialOutput.PermanentProducts)
                {
                    var permanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    var consumpterProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{permanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                    PermanentProductDataGrid productDataGrid = new PermanentProductDataGrid()
                    {
                        Id = item.ProductId,
                        Code = consumpterProduct.Code,
                        Description = consumpterProduct.Description,
                        Quantity = consumpterProduct.Stock,
                        QuantityAdded = 1,
                        Patrimony = permanentProduct.Patrimony,
                    };
                    this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalPermanentProductsAddedDataGrid.Items.Add(productDataGrid); }));
                }
            }
            catch (System.Net.Http.HttpRequestException) //DOESN'T EXISTS MATERIALOUTPUT REFERENCE ON WORKORDER
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocorreu um erro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            DocumentMovement.QueryMovement info = new DocumentMovement.QueryMovement()
            {
                Situation = (PrincipalMonitoring.Situation) ? "FECHADA" : "ABERTA",
                WorkOrder = PrincipalMonitoring.Work_Order,
                RegisterApplication = int.Parse(InfoUser.Register),
                SolicitationEmployee = InfoUser.Name
            };
            List<DocumentMovement.Product> products = new List<DocumentMovement.Product>();
            DocumentMovement document = new DocumentMovement(products, info);
        }
    }
}
