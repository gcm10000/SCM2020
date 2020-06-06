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
            ReferenceComboBox.Items.Add("Não utilizado");
            ReferenceComboBox.Items.Add("Transferência Interna");
            ReferenceComboBox.Items.Add("Outra Comarca");
        }

        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.Enter)
            {
                new Task(() => CheckOS()).Start();
            }
        }

        private void SearchOSButton_Click(object sender, RoutedEventArgs e)
        {
            new Task(() => CheckOS()).Start();
            //new Task(() => CheckOS()).Start();
            //this.ButtonInformation.IsHitTestVisible = false;
            //this.ButtonPermanentProducts.IsHitTestVisible = true;
            //this.ButtonFinish.IsHitTestVisible = true;

            //this.InfoScrollViewer.Visibility = Visibility.Visible;
            //this.InfoDockPanel.Visibility = Visibility.Visible;
            //this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
            //this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        }
        private void CheckOS()
        {
            string workorder = OSTextBox.Text;
            var uriRequest = new Uri(Helper.Server, $"monitoring/WorkOrder/{workorder}");

            Monitoring resultMonitoring;
            try
            {
                resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);

            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (resultMonitoring.Situation == false)
            {
                //ABERTA...
                GetProducts(resultMonitoring.Work_Order);

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
        private void GetProducts(string workorder)
        {
            //FALTA ADICIONAR USERCONTROL DE PRODUTO PERMANENTE!!
            //FALTA A SEGUNDA IDA NA SAÍDA...

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
                this.ConsumpterProductToAddDataGrid.Items.Add(consumpterProductDataGrid);
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
                };
                this.PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid);
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
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void VendorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateOS = OSDatePicker.SelectedDate ?? DateTime.Now;
            MaterialInput materialInput = new MaterialInput()
            {
                MovingDate = dateOS,
                Regarding = (Regarding)ReferenceComboBox.SelectedIndex,
                WorkOrder = OSDisableTextBox.Text,
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
                    Quantity = item.Quantity,
                    SCMRegistration = Helper.SCMRegistration,
                };
                materialInput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
            {
                AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                {
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    SCMRegistration = Helper.SCMRegistration
                };
                materialInput.PermanentProducts.Add(auxiliarPermanent);
            }
            var result = APIClient.PostData(new Uri(Helper.Server, "input/add").ToString(), materialInput, Helper.Authentication);
        }
        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PermanentProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void TxtProductConsumpterSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ConsumpterProductSearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
