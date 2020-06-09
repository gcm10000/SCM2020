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
        }
        class PermanentProductDataGrid : ConsumpterProductDataGrid
        {
            public string Patrimony { get; set; }
        }
        public Devolution()
        {
            InitializeComponent();
            //Fill combobox
            ReferenceComboBox.Items.Add("Não utilizado");
            ReferenceComboBox.Items.Add("Transferência Interna");
            ReferenceComboBox.Items.Add("Outra Comarca");

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

            Monitoring resultMonitoring;
            try
            {
                resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);
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

                this.ButtonInformation.IsHitTestVisible = false;
                this.ButtonPermanentProducts.IsHitTestVisible = true;
                this.ButtonFinish.IsHitTestVisible = true;

                this.InfoScrollViewer.Visibility = Visibility.Visible;
                this.InfoDockPanel.Visibility = Visibility.Visible;
                this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
                this.PermanentDockPanel.Visibility = Visibility.Collapsed;
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
                    QuantityAdded = productInputQuantity
                };
                this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Clear(); }));
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
                    //QuantityAdded = 1
                };
                this.PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid);
                this.ListPermanentProductDataGrid.Add(permanentProductDataGrid);
            }
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Refresh(); }));
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.UnselectAll(); }));
            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Refresh(); }));
            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.UnselectAll(); }));

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
            DateTime dateOS = OSDatePicker.SelectedDate ?? DateTime.Now;
            MaterialInput materialInput = new MaterialInput()
            {
                MovingDate = dateOS,
                Regarding = (Regarding)ReferenceComboBox.SelectedIndex,
                WorkOrder = OSTextBox.Text,
                DocDate = DateTime.Now,
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
            var result = APIClient.PostData(new Uri(Helper.Server, "devolution/add").ToString(), materialInput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as ConsumpterProductDataGrid;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                //int index = ProductToAddDataGrid.SelectedIndex;
                this.ConsumpterProductToAddDataGrid.Items.Refresh();
                this.FinalConsumpterProductsAddedDataGrid.Items.Refresh();
                if (!this.FinalConsumpterProductsAddedDataGrid.Items.Contains(product))
                    this.FinalConsumpterProductsAddedDataGrid.Items.Add(product);
                else
                {
                    if (dialog.QuantityAdded == 0)
                        this.FinalConsumpterProductsAddedDataGrid.Items.Remove(product);
                    else
                        product.QuantityAdded = dialog.QuantityAdded;
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
            var newList = list.Where(x => (x.Description == TxtProductConsumpterSearch.Text) || (x.Code.ToString() == TxtProductConsumpterSearch.Text));
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
            List<PermanentProductDataGrid> list = ListPermanentProductDataGrid;
            foreach (PermanentProductDataGrid item in this.PermanentProductToAddDataGrid.Items)
            {
                list.Add(item);
            }
            var newList = list.Where(x => (x.Description == TxtProductConsumpterSearch.Text) || (x.Code.ToString() == TxtProductConsumpterSearch.Text));
            this.PermanentProductToAddDataGrid.Items.Clear();
            this.PermanentProductToAddDataGrid.Items.Refresh();
            foreach (var item in newList)
            {
                this.PermanentProductToAddDataGrid.Items.Add(item);
            }
            this.PermanentProductToAddDataGrid.Items.Refresh();
            this.PermanentProductToAddDataGrid.UnselectAll();
        }
        private void OSTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string workorder = OSTextBox.Text;
            new Task(() => RescueData(workorder)).Start();
        }
        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string workorder = OSTextBox.Text;
                new Task(() => RescueData(workorder)).Start();
            }
        }
    }
}
