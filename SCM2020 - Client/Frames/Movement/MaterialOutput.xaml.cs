using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using WebAssemblyLibrary;

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
                ConsumptionProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{this.InformationProduct}").ToString(), Helper.Authentication);
            }
        }
        public Monitoring PrincipalMonitoring { get; set; }
        public InfoUser InfoUser { get; set; }
        WebBrowser WebBrowser = Helper.MyWebBrowser;
        public MaterialOutput()
        {
            InitializeComponent();
        }

        private bool previousOutputExists = false;
        private ModelsLibraryCore.MaterialOutput previousMaterialOutput = null;
        private void ConsumpterProductSearch(string query)
        {
            if (query == string.Empty)
                return;

            query = System.Uri.EscapeDataString(query);

            this.DataGridConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridConsumpterProducts.Items.Clear(); }));

            Uri uriProductsSearch = new Uri(Helper.ServerAPI, $"generalproduct/search/{query}");

            //Requisição de dados baseado na busca
            var products = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);

            foreach (var item in products.ToList())
            {
                ProductToOutput productsToOutput = new ProductToOutput()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    Quantity = item.Stock,
                };
                this.DataGridConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridConsumpterProducts.Items.Add(productsToOutput); }));
            }
        }
        private void PermanentProductSearch(string query)
        {
            if (query == string.Empty)
                return;

            query = System.Uri.EscapeDataString(query);


            Uri uriProductsSearch = new Uri(Helper.ServerAPI, $"PermanentProduct/search/{query}");

            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalPermanentProducts.Items.Clear(); }));

            var products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication).Where(x => string.IsNullOrWhiteSpace(x.WorkOrder));
            foreach (var product in products)
            {
                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid(new SearchPermanentProduct(product));
                this.DataGridPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridPermanentProducts.Items.Add(permanentProductDataGrid); }));
            }
        }
        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        //private void BtnInformation_Click(object sender, RoutedEventArgs e)
        //{
        //    this.ButtonInformation.IsHitTestVisible = false;
        //    this.ButtonPermanentProducts.IsHitTestVisible = true;
        //    this.ButtonFinish.IsHitTestVisible = true;

        //    this.InfoScrollViewer.Visibility = Visibility.Visible;
        //    this.InfoDockPanel.Visibility = Visibility.Visible;
        //    this.FinalProductsDockPanel.Visibility = Visibility.Collapsed;
        //    this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        //}
        //private void ButtonPermanentProducts_Click(object sender, RoutedEventArgs e)
        //{
        //    this.ButtonInformation.IsHitTestVisible = true;
        //    this.ButtonPermanentProducts.IsHitTestVisible = false;
        //    this.ButtonFinish.IsHitTestVisible = true;

        //    InfoScrollViewer.Visibility = Visibility.Collapsed;
        //    InfoDockPanel.Visibility = Visibility.Collapsed;
        //    PermanentDockPanel.Visibility = Visibility.Visible;
        //}
        //private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        //{
        //    this.ButtonInformation.IsHitTestVisible = true;
        //    this.ButtonPermanentProducts.IsHitTestVisible = true;
        //    this.ButtonFinish.IsHitTestVisible = false;

        //    this.InfoScrollViewer.Visibility = Visibility.Collapsed;
        //    this.InfoDockPanel.Visibility = Visibility.Collapsed;
        //    this.FinalProductsDockPanel.Visibility = Visibility.Visible;
        //    this.PermanentDockPanel.Visibility = Visibility.Collapsed;
        //}
        List<ProductToOutput> FinalConsumpterProductsAdded = new List<ProductToOutput>();
        List<ProductToOutput> FinalPermanentProductsAdded = new List<ProductToOutput>();

        //Método conveniente somente para produtos consumíveis
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = ((FrameworkElement)sender);
            var product = ((FrameworkElement)sender).DataContext as ProductToOutput;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                DataGridConsumpterProducts.Items.Refresh();
                DataGridFinalConsumpterProducts.Items.Refresh();
                if (!DataGridFinalConsumpterProducts.Items.Contains(product))
                {
                    product.NewProduct = button.Name == "BtnAdd";
                    DataGridFinalConsumpterProducts.Items.Add(product);
                    FinalConsumpterProductsAdded.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                    {
                        //this.previousMaterialOutput.ConsumptionProducts.Remove(product.ConsumptionProduct);
                        DataGridFinalConsumpterProducts.Items.Remove(product);
                        //FinalConsumpterProductsAdded.Remove(product);
                    }
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                    }
                    product.ProductChanged = true;
                }
                DataGridConsumpterProducts.UnselectAll();
                DataGridFinalConsumpterProducts.UnselectAll();
            }
        }
        private void ProductToAddDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            var currentRowIndex = DataGridConsumpterProducts.Items.IndexOf(DataGridConsumpterProducts.CurrentItem);
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
            DateTime dateTime = (DatePickerMovingDate.DisplayDate == DateTime.Today) ? (DateTime.Now) : DatePickerMovingDate.DisplayDate;

            var register = TextBoxRegisterApplicant.Text;
            string userId = string.Empty;
            try
            {
                userId = APIClient.GetData<string>(new Uri(Helper.ServerAPI, $"User/UserId/{register}").ToString());

            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
            var numberSector = TextBoxWorkOrder.Text.Substring(0, 2);
            Sector sector = null;
            try
            {
                sector = APIClient.GetData<Sector>(new Uri(Helper.ServerAPI, $"sector/NumberSector/{numberSector}").ToString(), Helper.Authentication);
                //COMPARAR SE O USUÁRIO É DO MESMO SETOR DA ORDEM DE SERVIÇO
                if (sector != Helper.CurrentSector)
                {
                    if (MessageBox.Show("Funcionário não é concernente com o setor da ordem de serviço." + Environment.NewLine + "Deseja continuar?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }
            catch
            {
                //Setor desconhecido...
                sector = APIClient.GetData<Sector>(new Uri(Helper.ServerAPI, $"sector/NumberSector/99").ToString(), Helper.Authentication);
            }
            Monitoring monitoring = new Monitoring()
            {
                SCMEmployeeId = Helper.NameIdentifier,
                Situation = false,
                ClosingDate = null,
                EmployeeId = userId,
                RequestingSector = sector.Id,//o setor é referente ao número da ordem de serviço,
                MovingDate = dateTime,
                Work_Order = TextBoxWorkOrder.Text,
                ServiceLocation = TextBoxServiceLocalization.Text
            };

            //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO

            var materialOutput = new ModelsLibraryCore.MaterialOutput
            {
                WorkOrder = TextBoxWorkOrder.Text,
                MovingDate = dateTime,
            };

            if (DataGridFinalConsumpterProducts.Items.Count > 0)
                materialOutput.ConsumptionProducts = new List<AuxiliarConsumption>();
            if (DataGridFinalPermanentProducts.Items.Count > 0)
                materialOutput.PermanentProducts = new List<AuxiliarPermanent>();

            foreach (var item in DataGridFinalConsumpterProducts.Items)
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
            foreach (var item in DataGridFinalPermanentProducts.Items)
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
                var workOrder = System.Uri.EscapeDataString(monitoring.Work_Order);

                var checkMonitoring = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"monitoring/checkworkorder/{workOrder}").ToString(), Helper.Authentication);
                if (!checkMonitoring)
                {
                    //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
                    var result1 = APIClient.PostData(new Uri(Helper.ServerAPI, "Monitoring/Add").ToString(), monitoring, Helper.Authentication);
                    MessageBox.Show(result1.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO

                var result2 = APIClient.PostData(new Uri(Helper.ServerAPI, "Output/Add").ToString(), materialOutput, Helper.Authentication);
                MessageBox.Show(result2.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);

                PrincipalMonitoring = monitoring;
            });
        }
        private void UpdateOutput()
        {
            ModelsLibraryCore.MaterialOutput materialOutput = this.previousMaterialOutput;
            if ((DataGridFinalConsumpterProducts.Items.Count > 0) && (materialOutput.ConsumptionProducts == null))
            {
                materialOutput.ConsumptionProducts = new List<AuxiliarConsumption>();
            }
            
            //materialOutput.PermanentProducts = new List<AuxiliarPermanent>();

            //var listConsumptionProduct = materialOutput.ConsumptionProducts.ToList();
            //var listPermanentProduct = materialOutput.PermanentProducts.ToList();

            //Loop baseado na lista de produtos consumíveis escolhidos
            foreach (ProductToOutput item in FinalConsumpterProductsAdded)
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
                    materialOutput.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    //listConsumptionProduct[FinalConsumpterProductsAdded.IndexOf(item)].Quantity =
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }

            foreach (var item in FinalPermanentProductsAdded)
            {
                //Se no DataGrid de produtos permanentes finais não conter o produto, será removido da lista
                if (!DataGridFinalPermanentProducts.Items.Contains(item))
                {
                    var permanentProduct = materialOutput.PermanentProducts.Single(x => x.ProductId == item.Id);
                    materialOutput.PermanentProducts.Remove(permanentProduct);
                }

                //Se na lista final não conter o produto permanente, então trate de adicionar.
                if (!materialOutput.PermanentProducts.Any(x => x.ProductId == item.Id))
                {
                    if (item.NewProduct)
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
            }

            Task.Run(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"output/Update/{materialOutput.Id}").ToString(), materialOutput, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DataGridFinalConsumpterProducts.Visibility = Visibility.Visible;
            this.DataGridFinalPermanentProducts.Visibility = Visibility.Collapsed;

            //this.ButtonFinalConsumpterProduct.IsHitTestVisible = false;
            //this.ButtonFinalPermanentProduct.IsHitTestVisible = true;
        }
        private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        {
            this.DataGridFinalPermanentProducts.Visibility = Visibility.Visible;
            this.DataGridFinalConsumpterProducts.Visibility = Visibility.Collapsed;

            //this.ButtonFinalConsumpterProduct.IsHitTestVisible = true;
            //this.ButtonFinalPermanentProduct.IsHitTestVisible = false;
        }
        //Método conveninente somente a produtos permanentes
        private void BtnAddRemovePermanent_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as PermanentProductDataGrid;
            if (product.BtnContent == "Adicionar")
            {
                product.QuantityAdded += 1;
                product.NewProduct = true;
                this.DataGridFinalPermanentProducts.Items.Add(product);
                this.FinalPermanentProductsAdded.Add(product);
                //product.BtnContent = "Remover";
            }
            else
            {
                //product.BtnContent = "Adicionar";
                product.QuantityAdded -= 1;
                product.NewProduct = false;
                this.DataGridFinalPermanentProducts.Items.Remove(product);
            }
            this.DataGridPermanentProducts.Items.Refresh();
            this.DataGridFinalPermanentProducts.Items.Refresh();
            this.DataGridPermanentProducts.UnselectAll();
            this.DataGridFinalPermanentProducts.UnselectAll();
        }
        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchConsumpterProduct.Text;
            Task.Run(() => ConsumpterProductSearch(query));
        }
        private void TxtSearchConsumpterProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearchConsumpterProduct.Text;
                Task.Run(() => ConsumpterProductSearch(query));
            }
        }
        private void PermanentProductSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchPermanentProduct.Text;
            Task.Run(() => { PermanentProductSearch(query); });
        }
        private void TxtPermanentProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearchPermanentProduct.Text;
                Task.Run(() => { PermanentProductSearch(query); });
            }
        }

        string previousOS = string.Empty;
        //KeyUp
        private void OSTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var workOrder = TextBoxWorkOrder.Text;
            if (previousOS == workOrder)
                return;
            Task.Run(() => RescueData(workOrder));
            previousOS = workOrder;

        }
        private void OSTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var workOrder = TextBoxWorkOrder.Text;
            if (e.Key == Key.Enter)
            {
                if (previousOS == workOrder)
                    return;
                Task.Run(() => RescueData(workOrder));
                previousOS = workOrder;
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

            DataToPrintORExportWasRescued = false;
            workOrder = System.Uri.EscapeDataString(workOrder);
            Monitoring monitoring = null;
            try
            {
                //Checar objeto monitoramento
                monitoring = APIClient.GetData<Monitoring>(new Uri(Helper.ServerAPI, $"Monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);

            }
            catch (System.Net.Http.HttpRequestException) //Ordem de serviço inexistente
            {
                ClearData();
                InputData(true);
                return;
            }

            try
            {
                var userId = monitoring.EmployeeId;
                var result = APIClient.GetData<string>(new Uri(Helper.ServerAPI, $"User/RegisterId/{userId}").ToString(), Helper.Authentication);
                InfoUser = APIClient.GetData<InfoUser>(new Uri(Helper.ServerAPI, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);

                PrincipalMonitoring = monitoring;
                if (monitoring.Situation) //Ordem de serviço encontra-se fechada.
                {
                    MessageBox.Show($"Ordem de serviço foi fechada na data {monitoring.ClosingDate.Value.ToString("dd/MM/yyyy")}.", "Ordem de serviço está fechada.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; //DISABLE FILLING DATA.
                }
                //ID -> MATRÍCULA
                var materialOutput = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.ServerAPI, $"Output/workOrder/{workOrder}").ToString(), Helper.Authentication);
                previousOutputExists = true;
                this.previousMaterialOutput = materialOutput;
                InputData(false);
                this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DatePickerMovingDate.SelectedDate = monitoring.MovingDate; this.DatePickerMovingDate.DisplayDate = monitoring.MovingDate; }));
                this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxServiceLocalization.Text = monitoring.ServiceLocation; }));

                this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxRegisterApplicant.Text = InfoUser.Register; }));
                this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxApplicant.Text = InfoUser.Name; }));

                this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumpterProducts.Items.Clear(); }));
                this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalPermanentProducts.Items.Clear(); }));
                foreach (var item in materialOutput.ConsumptionProducts)
                {
                    var consumpterProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    ProductToOutput productToOutput = new ProductToOutput()
                    {
                        Id = item.ProductId,
                        QuantityAdded = item.Quantity,
                        Description = consumpterProduct.Description,
                        Code = consumpterProduct.Code,
                        Quantity = consumpterProduct.Stock,
                        ConsumptionProduct = item
                    };
                    this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action (() => { DataGridFinalConsumpterProducts.Items.Add(productToOutput); }));
                    FinalConsumpterProductsAdded.Add(productToOutput);
                }
                foreach (var item in materialOutput.PermanentProducts)
                {
                    var permanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.ServerAPI, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                    var consumpterProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{permanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                    PermanentProductDataGrid productDataGrid = new PermanentProductDataGrid()
                    {
                        Id = item.ProductId,
                        Code = consumpterProduct.Code,
                        Description = consumpterProduct.Description,
                        Quantity = consumpterProduct.Stock,
                        QuantityAdded = 1,
                        Patrimony = permanentProduct.Patrimony,
                    };
                    this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalConsumpterProducts.Items.Add(productDataGrid); }));
                    FinalPermanentProductsAdded.Add(productDataGrid);
                }

                this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));
                this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
            }
            catch (System.Net.Http.HttpRequestException) //Não existe saída de material nesta ordem de serviço.
            {
                InputData(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocorreu um Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ClearData()
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.Text = string.Empty; }));
            this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxApplicant.Text = string.Empty; }));
            this.ComboBoxOutputType.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxOutputType.SelectedIndex = 0; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.Text = string.Empty; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = DateTime.Now; }));
            this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumpterProducts.Items.Clear(); }));
            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalPermanentProducts.Items.Clear(); }));
        }

        private void InputData(bool IsEnable)
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.IsEnabled = IsEnable; }));
            //this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ApplicantTextBox.IsEnabled = IsEnable; }));
            this.ComboBoxOutputType.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxOutputType.IsEnabled = IsEnable; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.IsEnabled = IsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = IsEnable; }));
        }

        bool DataToPrintORExportWasRescued = false;
        bool PrintORExport = false;
        string Document = string.Empty;

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!DataToPrintORExportWasRescued)
            {
                DataToPrintORExportWasRescued = true;
            }
            SCM2020___Client.Templates.Movement.MaterialMovement movement = new Templates.Movement.MaterialMovement(PrincipalMonitoring.Work_Order);
            PrintORExport = true;
            Document = movement.RenderizeHtml();
            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.WebBrowser.NavigateToString(Document);

        }
        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Helper.SetOptionsToPrint();
            if (PrintORExport)
            {
                WebBrowser.PrintDocument();
            }
            else
            {
                string printer = Helper.GetPrinter("PDF");
                string tempFile = string.Empty;
                try
                {

                    tempFile = Helper.GetTempFilePathWithExtension(".tmp");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(tempFile, true))
                    {
                        file.Write(Document);
                        file.Flush();
                    }

                    //"f=" The input file
                    //"p=" The temporary default printer
                    //"d|delete" Delete file when finished
                    var p = new Process();
                    p.StartInfo.FileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Exporter\\document-exporter.exe");
                    //Fazer com que o document-exporter apague o arquivo após a impressão. Ao invés de utilizar finally. Motivo é evitar que o arquivo seja apagado antes do Document-Exporter possa lê-lo.
                    p.StartInfo.Arguments = $"-p=\"{printer}\" -f=\"{tempFile}\" -d";
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro durante exportação", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            this.WebBrowser.LoadCompleted -= WebBrowser_LoadCompleted;
        }


        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            if (!DataToPrintORExportWasRescued)
            {
                DataToPrintORExportWasRescued = true;
            }

            SCM2020___Client.Templates.Movement.MaterialMovement movement = new Templates.Movement.MaterialMovement(PrincipalMonitoring.Work_Order);

            PrintORExport = false;
            Document = movement.RenderizeHtml();
            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.WebBrowser.NavigateToString(Document);
        }
        private void GetName(string register)
        {
            if (string.IsNullOrWhiteSpace(register))
                return;
            try
            {
                //Zera informação
                this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxApplicant.Text = string.Empty; }));
                //Recebe informações
                var infoUser = APIClient.GetData<InfoUser>(new Uri(Helper.ServerAPI, $"user/InfoUserRegister/{register}").ToString(), Helper.Authentication);
                //Atribui o nome do funcionário no campo correspondente
                this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxApplicant.Text = infoUser.Name; }));
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterApplicantTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string register = this.TextBoxRegisterApplicant.Text;
                Task.Run(() => GetName(register));
            }
        }

        private void RegisterApplicantTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string register = this.TextBoxRegisterApplicant.Text;
            Task.Run(() => GetName(register));
        }

        private void ButtonPrevious1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPrevious2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxWorkOrder_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void DatePickerMovingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxOutputType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonNext1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridConsumpterProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGridPermanentProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGridFinalConsumpterProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGridFinalPermanentProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
