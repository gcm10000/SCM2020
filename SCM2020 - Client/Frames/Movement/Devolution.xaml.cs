using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.Frames.Query;
using System;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SCM2020___Client.Frames.Movement
{
    /// <summary>
    /// Interação lógica para Devolution.xam
    /// </summary>
    public partial class Devolution : UserControl
    {
        class ConsumpterProductDataGrid
        {
            //public string Image { get; set; }
            public int Id { get; set; }
            public int Code { get; set; }
            public double QuantityFuture { get => Quantity + QuantityAdded; }
            public double QuantityOutput { get; set; }
            public double QuantityAdded { get; set; }
            public double Quantity { get; set; }
            public string Description { get; set; }
            public bool NewProduct { get; set; }
            public bool ProductChanged { get; set; }
            public ModelsLibraryCore.AuxiliarConsumption ConsumptionProduct { get; set; }
        }
        class PermanentProductDataGrid : ConsumpterProductDataGrid
        {
            public string Patrimony { get; set; }
            public string BtnContent { get => (QuantityAdded == 1) ? "Remover" : "Adicionar"; }
        }
        bool previousDevolutionExists = false;
        MaterialInput previousMaterialInput = null;
        public Devolution()
        {
            InitializeComponent();
        }
        //private void SearchOSButton_Click(object sender, RoutedEventArgs e)
        //{
        //    new Task(() => CheckOS()).Start();
        //    //new Task(() => CheckOS()).Start();
        //    //this.ButtonInformation.IsHitTestVisible = false;
        //    //this.ButtonPermanentProducts.IsHitTestVisible = true;
        //    //this.ButtonFinish.IsHitTestVisible = true;

        //    //this.InfoScrollViewer.Visibility = Visibility.Visible;
        //    //this.InfoDockPanel.Visibility = Visibility.Visible;
        //    //this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
        //    //this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        //}
        private void RescueData(string workOrder)
        {
            workOrder = System.Uri.EscapeDataString(workOrder);
            var uriRequest = new Uri(Helper.Server, $"monitoring/WorkOrder/{workOrder}");
            string teste = uriRequest.ToString();
            Monitoring resultMonitoring;
            try
            {
                resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);
                var register = resultMonitoring.EmployeeId;
                var infoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{register}").ToString(), Helper.Authentication);
                this.RegisterApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicantTextBox.Text = infoUser.Register; }));
                this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicantTextBox.Text = infoUser.Name; }));
                //this.RegisterApplicantTextBox.Text = infoUser.Register;
                //this.ApplicantTextBox.Text = infoUser.Name;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Ocorreu um erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocorreu um erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (resultMonitoring.Situation == false)
            {
                //ABERTA...

                GetProducts(workOrder);
                try
                {

                }
                catch (System.Net.Http.HttpRequestException)
                {

                }
                this.ButtonInformation.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonInformation.IsHitTestVisible = false; }));
                this.ButtonPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPermanentProducts.IsHitTestVisible = true; }));
                this.ButtonFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonFinish.IsHitTestVisible = true; }));
                
                this.InfoScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoScrollViewer.Visibility = Visibility.Visible; }));
                this.InfoDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoDockPanel.Visibility = Visibility.Visible; }));
                this.FinalProductsDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalProductsDockPanel.Visibility = Visibility.Collapsed; }));
                this.PermanentDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentDockPanel.Visibility = Visibility.Collapsed; }));

            }
            else
            {
                DateTime closingDate = resultMonitoring.ClosingDate ?? DateTime.Now;
                MessageBox.Show($"Ordem de serviço foi fechada na data {closingDate.ToString("dd-MM-YYYY")}.", "Ordem de serviço está fechada.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        List<ConsumpterProductDataGrid> ListConsumpterProductDataGrid = new List<ConsumpterProductDataGrid>();
        List<PermanentProductDataGrid> ListPermanentProductDataGrid = new List<PermanentProductDataGrid>();
        private void GetProducts(string workorder)
        {
            var outputProducts = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.Server, $"output/workorder/{workorder}").ToString(), Helper.Authentication);
            ListConsumpterProductDataGrid.Clear();
            ListPermanentProductDataGrid.Clear();
            
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Clear(); }));
            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Clear(); }));

            foreach (var item in outputProducts.ConsumptionProducts)
            {
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                AuxiliarConsumption productInput;
                double productInputQuantity = 0.00d;
                try
                {
                    var infoInput = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.Server, $"input/workorder/{workorder}").ToString(), Helper.Authentication);
                    productInput = infoInput.ConsumptionProducts.First(x => x.ProductId == item.ProductId);
                    productInputQuantity = productInput.Quantity;
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    //Devolução de item na ordem de serviço é inexistente
                }
                ConsumpterProductDataGrid consumpterProductDataGrid = new ConsumpterProductDataGrid()
                {
                    Code = infoProduct.Code,
                    Description = infoProduct.Description,
                    Id = item.ProductId,
                    Quantity = infoProduct.Stock,
                    QuantityOutput = item.Quantity,
                    QuantityAdded = productInputQuantity,
                    NewProduct = false,
                    ConsumptionProduct = item
                };

                this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Add(consumpterProductDataGrid); }));
                this.ListConsumpterProductDataGrid.Add(consumpterProductDataGrid);
            }
            foreach (var item in outputProducts.PermanentProducts)
            {
                var infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);

                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
                {
                    Code = infoProduct.Code,
                    Description = infoProduct.Description,
                    //Id do produto permanente
                    Id = item.ProductId,
                    Quantity = infoProduct.Stock,
                    Patrimony = infoPermanentProduct.Patrimony,
                    QuantityOutput = 1,
                    NewProduct = false
                    //QuantityAdded = 1
                };
                this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid); }));
                this.ListPermanentProductDataGrid.Add(permanentProductDataGrid);
            }
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Refresh(); }));
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.UnselectAll(); }));
            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Refresh(); }));
            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.UnselectAll(); }));

            try
            {
                MaterialInput materialInput = APIClient.GetData<MaterialInput>(new Uri(Helper.Server, $"devolution/workorder/{workorder}").ToString(), Helper.Authentication);
                this.previousDevolutionExists = true;
                this.previousMaterialInput = materialInput;
                this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ReferenceComboBox.SelectedIndex = (int)(materialInput.Regarding + 1); }));

                this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    this.FinalConsumpterProductsAddedDataGrid.Items.Clear();
                    foreach (var item in materialInput.ConsumptionProducts)
                    {
                        var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        ConsumpterProductDataGrid consumpterProductDataGrid = new ConsumpterProductDataGrid()
                        {
                            Id = item.ProductId,
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Quantity = infoProduct.Stock,
                            QuantityAdded = item.Quantity,
                            NewProduct = false,
                            ConsumptionProduct = item
                            //QuantityOutput
                        };
                        this.FinalConsumpterProductsAddedDataGrid.Items.Add(consumpterProductDataGrid);
                    }
                }));
                this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    this.FinalPermanentProductsAddedDataGrid.Items.Clear();
                    foreach (var item in materialInput.PermanentProducts)
                    {
                        var infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                        var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                        PermanentProductDataGrid consumpterProductDataGrid = new PermanentProductDataGrid()
                        {
                            Id = item.ProductId,
                            Code = infoProduct.Code,
                            Description = infoProduct.Description,
                            Quantity = infoProduct.Stock,
                            Patrimony = infoPermanentProduct.Patrimony,
                            //QuantityOutput = 1,
                            QuantityAdded = 1,
                        };
                        this.FinalPermanentProductsAddedDataGrid.Items.Add(consumpterProductDataGrid);
                    }
                }));
            }
            catch (System.Net.Http.HttpRequestException)
            {
                //DOESNOT EXIST INPUT (DEVOLUTION) REFERENCE FOR WORKORDER
                previousDevolutionExists = false;
            }
            catch (Exception ex)
            {
                previousDevolutionExists = false;
                MessageBox.Show(ex.Message, "Ocorreu um erro.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonInformation_Click(object sender, RoutedEventArgs e)
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
        private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Visible;
            this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Collapsed;

            this.ButtonFinalConsumpterProduct.IsHitTestVisible = false;
            this.ButtonFinalPermanentProduct.IsHitTestVisible = true;
        }
        private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        {
            this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Collapsed;
            this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Visible;

            this.ButtonFinalConsumpterProduct.IsHitTestVisible = true;
            this.ButtonFinalPermanentProduct.IsHitTestVisible = false;
        }
        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (previousDevolutionExists)
                UpdateInput();
            else
                AddInput();

        }
        private void AddInput()
        {
            MaterialInput materialInput = new MaterialInput()
            {
                Regarding = (Regarding)ReferenceComboBox.SelectedIndex,
                WorkOrder = OSTextBox.Text,
                //DocDate não deveria existir. Tratamentos diretos sobre ordem de serviço constitui-se na classe Monitoring.
                DocDate = DateTime.Now,
                //Data da criação de objeto
                //ou seja, da primeira movimentação
                MovingDate = DateTime.Now,
            };

            if (FinalConsumpterProductsAddedDataGrid.Items.Count > 0)
                materialInput.ConsumptionProducts = new List<AuxiliarConsumption>();
            if (FinalPermanentProductsAddedDataGrid.Items.Count > 0)
                materialInput.PermanentProducts = new List<AuxiliarPermanent>();
            foreach (ConsumpterProductDataGrid item in FinalConsumpterProductsAddedDataGrid.Items)
            {
                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    Quantity = item.QuantityAdded,
                    SCMEmployeeId = Helper.SCMId,
                };
                materialInput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
            {
                AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                {
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    SCMEmployeeId = Helper.SCMId
                };
                materialInput.PermanentProducts.Add(auxiliarPermanent);
            }
            //TASK...
            var result = APIClient.PostData(new Uri(Helper.Server, "devolution/add").ToString(), materialInput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void UpdateInput()
        {
            MaterialInput materialInput = this.previousMaterialInput;
            if ((FinalConsumpterProductsAddedDataGrid.Items.Count > 0) && (materialInput.ConsumptionProducts == null))
            {
                materialInput.ConsumptionProducts = new List<AuxiliarConsumption>();
            }
            if ((FinalPermanentProductsAddedDataGrid.Items.Count > 0) && (materialInput.PermanentProducts == null))
            {
                materialInput.PermanentProducts = new List<AuxiliarPermanent>();
            }
            foreach (ConsumpterProductDataGrid item in FinalConsumpterProductsAddedDataGrid.Items)
            {
                if (item.NewProduct)
                {
                    AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        Quantity = item.QuantityAdded,
                        SCMEmployeeId = Helper.SCMId,
                    };
                    materialInput.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
            {
                if (!materialInput.PermanentProducts.Any(x => x.ProductId == item.Id))
                {
                    AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        SCMEmployeeId = Helper.SCMId
                    };
                    materialInput.PermanentProducts.Add(auxiliarPermanent);
                }
            }
            var result = APIClient.PostData(new Uri(Helper.Server, $"devolution/Update/{materialInput.Id}").ToString(), this.previousMaterialInput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = ((FrameworkElement)sender);
            var product = ((FrameworkElement)sender).DataContext as ConsumpterProductDataGrid;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                //int index = ProductToAddDataGrid.SelectedIndex;
                this.ConsumpterProductToAddDataGrid.Items.Refresh();
                this.FinalConsumpterProductsAddedDataGrid.Items.Refresh();
                if (!this.FinalConsumpterProductsAddedDataGrid.Items.Contains(product))
                {
                    product.NewProduct = button.Name == "BtnToAdd";
                    this.FinalConsumpterProductsAddedDataGrid.Items.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                    {
                        this.previousMaterialInput.ConsumptionProducts.Remove(product.ConsumptionProduct);
                        this.FinalConsumpterProductsAddedDataGrid.Items.Remove(product);
                    }
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                        product.ProductChanged = true;
                    }
                }
                this.ConsumpterProductToAddDataGrid.UnselectAll();
                this.FinalConsumpterProductsAddedDataGrid.UnselectAll();
            }
        }
        private void PermanentProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        private void TxtProductConsumpterSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConsumpterProductSearchButton_Click(sender, e);
            }
        }
        private void ConsumpterProductSearchButton_Click(object sender, RoutedEventArgs e)
        {
            //TxtProductConsumpterSearch_KeyDown
            //fazer com que filtre dentro do datagrid
            //this.ConsumpterProductToAddDataGrid
            List<ConsumpterProductDataGrid> list = ListConsumpterProductDataGrid;
            var newList = list.Where(x => (x.Description.Contains(TxtProductConsumpterSearch.Text)) || (x.Code.ToString().Contains(TxtProductConsumpterSearch.Text)));
            this.ConsumpterProductToAddDataGrid.Items.Clear();
            this.ConsumpterProductToAddDataGrid.Items.Refresh();
            foreach (var item in newList)
            {
                this.ConsumpterProductToAddDataGrid.Items.Add(item);
            }
            this.ConsumpterProductToAddDataGrid.Items.Refresh();
            this.ConsumpterProductToAddDataGrid.UnselectAll();
        }
        private void TxtPermanentProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PermanentProductButton_Click(sender, e);
            }
        }
        private void PermanentProductButton_Click(object sender, RoutedEventArgs e)
        {
            List<PermanentProductDataGrid> list = new List<PermanentProductDataGrid>(ListPermanentProductDataGrid);
            var newList = list.Where(x => (x.Description.Contains(TxtPermanentProductSearch.Text) ) || (x.Code.ToString().Contains(TxtPermanentProductSearch.Text) || (x.Patrimony.Contains(TxtPermanentProductSearch.Text))));
            this.PermanentProductToAddDataGrid.Items.Clear();
            this.PermanentProductToAddDataGrid.Items.Refresh();
            foreach (var item in newList)
            {
                this.PermanentProductToAddDataGrid.Items.Add(item);
            }
            this.PermanentProductToAddDataGrid.Items.Refresh();
            this.PermanentProductToAddDataGrid.UnselectAll();
        }
        string oldOS = string.Empty;
        private void OSTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string workorder = OSTextBox.Text;
            if (oldOS == workorder)
                return;
            else
                oldOS = workorder;
            new Task(() => RescueData(workorder)).Start();
        }
        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string workorder = OSTextBox.Text;
                if (oldOS == workorder)
                    return;
                else
                    oldOS = workorder;
                new Task(() => RescueData(workorder)).Start();
            }
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
    }
}
