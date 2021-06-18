using MaterialDesignThemes.Wpf;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.Frames.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using WebAssemblyLibrary;
using Monitoring = SCM2020___Client.Models.Monitoring;

namespace SCM2020___Client.Frames.Movement
{
    /// <summary>
    /// Interação lógica para Devolution.xam
    /// </summary>
    public partial class Devolution : UserControl
    {

        public MenuItem CurrentMenuItem
        {
            get => currentMenuItem;
            private set
            {
                if (value != null)
                {
                    currentMenuItem = value;
                    switch (value.IdEvent.ToString())
                    {
                        case "0":
                            ShowInfo();
                            break;
                        case "1":
                            ShowConsumpterProducts();
                            break;
                        case "2":
                            ShowPermanentProducts();
                            break;
                        case "3":
                            ShowFinish();
                            break;
                    }
                    ScreenChanged?.Invoke(value, new EventArgs());

                }
            }
        }

        private MenuItem currentMenuItem;
        public List<MenuItem> Menu { get; private set; }
        public delegate void MenuDelegate(object sender, EventArgs e);
        public event MenuDelegate ScreenChanged;
        public delegate void MenuItemEnabled(int IdEvent, bool IsEnabled);
        public event MenuItemEnabled MenuItemEventHandler;

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            CurrentMenuItem = Menu[int.Parse(button.Uid)];
        }

        private void ShowInfo()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Visible;
            this.GridConsumpterProducts.Visibility = Visibility.Collapsed;
            this.GridPermanentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }

        private void ShowConsumpterProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumpterProducts.Visibility = Visibility.Visible;
            this.GridPermanentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }

        private void ShowPermanentProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumpterProducts.Visibility = Visibility.Collapsed;
            this.GridPermanentProducts.Visibility = Visibility.Visible;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }

        private void ShowFinish()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumpterProducts.Visibility = Visibility.Collapsed;
            this.GridPermanentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Visible;

            List<SummaryInfo> infos = new List<SummaryInfo>()
            {
                new SummaryInfo("DOC/SM/OS", TextBoxWorkOrder.Text, PackIconKind.Invoice),
                new SummaryInfo("Data da Ordem de Serviço", DatePickerMovingDate.SelectedDate.Value.ToString("dd/MM/yyyy"), PackIconKind.CalendarToday),
                new SummaryInfo("Local de Serviço", TextBoxServiceLocalization.Text, PackIconKind.Work),
                new SummaryInfo("Nome do Solicitante", TextBoxApplicant.Text, PackIconKind.Person),
            };
            this.ListView.ItemsSource = infos;
        }

        class ConsumpterProductDataGrid
        {
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
        //bool previousDevolutionExists = false;
        Monitoring PrincipalMonitoring;
        WebBrowser WebBrowser = Helper.MyWebBrowser;
        MaterialInput previousMaterialInput = null;
        bool PrintORExport = false;
        string Document = string.Empty;

        public Devolution()
        {
            InitializeComponent();

            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Informações", 0, true),
                new MenuItem(Name: "Produtos Consumíveis", 1, false),
                new MenuItem(Name: "Produtos Permanentes", 2, false),
                new MenuItem(Name: "Finalização", 3, false)
            };
            CurrentMenuItem = Menu[0];
            this.ComboBoxReference.SelectedIndex = 0;
            this.DatePickerMovingDate.SelectedDate = DateTime.Now;
        }

        private void RescueData(string workOrder)
        {
            if (workOrder == string.Empty)
                return;

            this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(() => { this.DataGridFinalConsumpterProducts.Items.Clear(); });
            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(() => { this.DataGridFinalPermanentProducts.Items.Clear(); });
            workOrder = System.Uri.EscapeDataString(workOrder);
            var uriRequest = new Uri(Helper.ServerAPI, $"monitoring/WorkOrder/{workOrder}");
            Monitoring resultMonitoring = null;
            
            //previousDevolutionExists = false;
            try
            {
                resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);
                var userId = resultMonitoring.EmployeeId;
                var infoUser = APIClient.GetData<InfoUser>(new Uri(Helper.ServerAPI, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
                this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxRegisterApplicant.Text = infoUser.Register; }));
                this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxApplicant.Text = infoUser.Name; }));
                this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DatePickerMovingDate.SelectedDate = resultMonitoring.MovingDate; this.DatePickerMovingDate.DisplayDate = resultMonitoring.MovingDate; }));
                this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxServiceLocalization.Text = resultMonitoring.ServiceLocation; }));
                PrincipalMonitoring = resultMonitoring;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                //Não existe monitoramento com este ordem de serviço
                //Sempre será capturado o código 204 NO CONTENT
                ClearData();
                InputData(true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocorreu um erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Se o monitoramento vinculado a ordem de serviço é existente
            if (resultMonitoring != null)
            {
                if (resultMonitoring.Situation == false) //Se a ordem de serviço encontra-se aberta
                {
                    try
                    {
                        previousMaterialInput = APIClient.GetData<MaterialInput>(new Uri(Helper.ServerAPI, $"devolution/workorder/{workOrder}").ToString(), Helper.Authentication);
                        //previousDevolutionExists = true;
                        this.ComboBoxReference.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ComboBoxReference.SelectedIndex = ((int)previousMaterialInput.Regarding) - 1; }));

                        RescueProducts(previousMaterialInput);
                        InputData(false, false);

                        this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));
                        this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
                    }

                    catch (System.Net.Http.HttpRequestException) //Não existe entrada nesta ordem de serviço
                    {
                        InputData(true, false, false, false);
                    }

                }
                else
                {
                    DateTime closingDate = resultMonitoring.ClosingDate ?? DateTime.Now;
                    MessageBox.Show($"Ordem de serviço foi fechada na data {closingDate.ToString("dd/MM/yyyy")}.", "Ordem de serviço está fechada.", MessageBoxButton.OK, MessageBoxImage.Error);
                    InputData(false, false);
                    ClearData();
                }
            }
        }

        private void ClearData()
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.Text = string.Empty; }));
            this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxApplicant.Text = string.Empty; }));
            this.ComboBoxReference.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxReference.SelectedIndex = 0; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.Text = string.Empty; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = DateTime.Now; }));

            this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalConsumpterProducts.Items.Clear(); }));
            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalPermanentProducts.Items.Clear(); }));
        }

        private void InputData(bool IsEnable, bool OSDatePickerIsEnable)
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.IsEnabled = IsEnable; }));
            this.ComboBoxReference.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxReference.IsEnabled = IsEnable; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.IsEnabled = IsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = OSDatePickerIsEnable; }));
        }

        private void InputData(bool ReferenceComboBoxIsEnable, bool OSDatePickerIsEnable, bool RegisterApplicantTextBoxIsEnable, bool ServiceLocalizationIsEnable)
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.IsEnabled = RegisterApplicantTextBoxIsEnable; }));
            this.ComboBoxReference.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxReference.IsEnabled = ReferenceComboBoxIsEnable; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.IsEnabled = ServiceLocalizationIsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = OSDatePickerIsEnable; }));
        }

        private void RescueProducts(ModelsLibraryCore.MaterialInput materialInput)
        {
            List<ConsumpterProductDataGrid> consumpterProducts = new List<ConsumpterProductDataGrid>();

            foreach (var item in materialInput.ConsumptionProducts)
            {
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
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
                if (item.Quantity != 0)
                {
                    consumpterProducts.Add(consumpterProductDataGrid);
                }
                this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalConsumpterProducts.Items.Add(consumpterProductDataGrid); }));
            }

            //this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalConsumpterProducts.ItemsSource = consumpterProducts; }));
            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalPermanentProducts.Items.Clear(); }));

            foreach (var item in materialInput.PermanentProducts)
            {
                var infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.ServerAPI, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                PermanentProductDataGrid consumpterProductDataGrid = new PermanentProductDataGrid()
                {
                    Id = item.ProductId,
                    Code = infoProduct.Code,
                    Description = infoProduct.Description,
                    Quantity = infoProduct.Stock,
                    Patrimony = infoPermanentProduct.Patrimony,
                    QuantityAdded = 1,
                };
                this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalPermanentProducts.Items.Add(consumpterProductDataGrid); }));
            }

            this.previousMaterialInput = materialInput;
        }
        //private void GetProducts(string workorder)
        //{
        //    var outputProducts = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.Server, $"output/workorder/{workorder}").ToString(), Helper.Authentication);
        //    ListConsumpterProductDataGrid.Clear();
        //    ListPermanentProductDataGrid.Clear();

        //    this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Clear(); }));
        //    this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Clear(); }));

        //    foreach (var item in outputProducts.ConsumptionProducts)
        //    {
        //        var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
        //        AuxiliarConsumption productInput;
        //        double productInputQuantity = 0.00d;
        //        try
        //        {
        //            var infoInput = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.Server, $"input/workorder/{workorder}").ToString(), Helper.Authentication);
        //            productInput = infoInput.ConsumptionProducts.First(x => x.ProductId == item.ProductId);
        //            productInputQuantity = productInput.Quantity;
        //        }
        //        catch (System.Net.Http.HttpRequestException)
        //        {
        //            //Devolução de item na ordem de serviço é inexistente
        //        }
        //        ConsumpterProductDataGrid consumpterProductDataGrid = new ConsumpterProductDataGrid()
        //        {
        //            Code = infoProduct.Code,
        //            Description = infoProduct.Description,
        //            Id = item.ProductId,
        //            Quantity = infoProduct.Stock,
        //            QuantityOutput = item.Quantity,
        //            QuantityAdded = productInputQuantity,
        //            NewProduct = false,
        //            ConsumptionProduct = item
        //        };

        //        this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Add(consumpterProductDataGrid); }));
        //        this.ListConsumpterProductDataGrid.Add(consumpterProductDataGrid);
        //    }
        //    foreach (var item in outputProducts.PermanentProducts)
        //    {
        //        var infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
        //        var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);

        //        PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
        //        {
        //            Code = infoProduct.Code,
        //            Description = infoProduct.Description,
        //            //Id do produto permanente
        //            Id = item.ProductId,
        //            Quantity = infoProduct.Stock,
        //            Patrimony = infoPermanentProduct.Patrimony,
        //            QuantityOutput = 1,
        //            NewProduct = false
        //            //QuantityAdded = 1
        //        };
        //        this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid); }));
        //        this.ListPermanentProductDataGrid.Add(permanentProductDataGrid);
        //    }
        //    this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.Items.Refresh(); }));
        //    this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ConsumpterProductToAddDataGrid.UnselectAll(); }));
        //    this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.Items.Refresh(); }));
        //    this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentProductToAddDataGrid.UnselectAll(); }));

        //    try
        //    {
        //        MaterialInput materialInput = APIClient.GetData<MaterialInput>(new Uri(Helper.Server, $"devolution/workorder/{workorder}").ToString(), Helper.Authentication);
        //        this.previousDevolutionExists = true;
        //        this.previousMaterialInput = materialInput;
        //        this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ReferenceComboBox.SelectedIndex = (int)(materialInput.Regarding + 1); }));

        //        this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
        //        {
        //            this.FinalConsumpterProductsAddedDataGrid.Items.Clear();
        //            foreach (var item in materialInput.ConsumptionProducts)
        //            {
        //                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
        //                ConsumpterProductDataGrid consumpterProductDataGrid = new ConsumpterProductDataGrid()
        //                {
        //                    Id = item.ProductId,
        //                    Code = infoProduct.Code,
        //                    Description = infoProduct.Description,
        //                    Quantity = infoProduct.Stock,
        //                    QuantityAdded = item.Quantity,
        //                    NewProduct = false,
        //                    ConsumptionProduct = item
        //                    //QuantityOutput
        //                };
        //                if (item.Quantity != 0)
        //                    this.FinalConsumpterProductsAddedDataGrid.Items.Add(consumpterProductDataGrid);
        //                ConsumpterProductInput.Add(consumpterProductDataGrid);
        //            }
        //        }));
        //        this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
        //        {
        //            this.FinalPermanentProductsAddedDataGrid.Items.Clear();
        //            foreach (var item in materialInput.PermanentProducts)
        //            {
        //                var infoPermanentProduct = APIClient.GetData<ModelsLibraryCore.PermanentProduct>(new Uri(Helper.Server, $"permanentproduct/{item.ProductId}").ToString(), Helper.Authentication);
        //                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{infoPermanentProduct.InformationProduct}").ToString(), Helper.Authentication);
        //                PermanentProductDataGrid consumpterProductDataGrid = new PermanentProductDataGrid()
        //                {
        //                    Id = item.ProductId,
        //                    Code = infoProduct.Code,
        //                    Description = infoProduct.Description,
        //                    Quantity = infoProduct.Stock,
        //                    Patrimony = infoPermanentProduct.Patrimony,
        //                    //QuantityOutput = 1,
        //                    QuantityAdded = 1,
        //                };
        //                this.FinalPermanentProductsAddedDataGrid.Items.Add(consumpterProductDataGrid);
        //            }
        //        }));
        //    }
        //    catch (System.Net.Http.HttpRequestException)
        //    {
        //        //DOESNOT EXIST INPUT (DEVOLUTION) REFERENCE FOR WORKORDER
        //        previousDevolutionExists = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        previousDevolutionExists = false;
        //        MessageBox.Show(ex.Message, "Ocorreu um erro.", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //private void ButtonInformation_Click(object sender, RoutedEventArgs e)
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
        //private void ButtonFinalConsumpterProduct_Click(object sender, RoutedEventArgs e)
        //{
        //    this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Visible;
        //    this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Collapsed;

        //    this.ButtonFinalConsumpterProduct.IsHitTestVisible = false;
        //    this.ButtonFinalPermanentProduct.IsHitTestVisible = true;
        //}
        //private void ButtonFinalPermanentProduct_Click(object sender, RoutedEventArgs e)
        //{
        //    this.FinalConsumpterProductsAddedDataGrid.Visibility = Visibility.Collapsed;
        //    this.FinalPermanentProductsAddedDataGrid.Visibility = Visibility.Visible;

        //    this.ButtonFinalConsumpterProduct.IsHitTestVisible = true;
        //    this.ButtonFinalPermanentProduct.IsHitTestVisible = false;
        //}


        //private void BtnFinish_Click(object sender, RoutedEventArgs e)
        //{
        //    if (previousDevolutionExists)
        //        UpdateInput();
        //    else
        //        AddInput();

        //}

        private void AddMonitoring(DateTime dateTime, string register, string numberSector, string workOrder, string serviceLocation)
        {

            string userId = string.Empty;
            try
            {
                userId = APIClient.GetData<string>(new Uri(Helper.ServerAPI, $"User/UserId/{register}").ToString(), Helper.Authentication);

            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
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
                Work_Order = workOrder,
                ServiceLocation = serviceLocation
            };

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
            var result = APIClient.PostData(new Uri(Helper.ServerAPI, "Monitoring/Add").ToString(), monitoring, Helper.Authentication);
            MessageBox.Show(result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);

            PrincipalMonitoring = monitoring;
        }

        private void AddInput(Regarding regarding, DateTime dateTime, string workOrder)
        {

            MaterialInput materialInput = new MaterialInput()
            {
                Regarding = regarding,
                WorkOrder = workOrder,
                MovingDate = dateTime,
            };

            if (DataGridFinalConsumpterProducts.Items.Count > 0)
                materialInput.ConsumptionProducts = new List<AuxiliarConsumption>();
            if (DataGridFinalPermanentProducts.Items.Count > 0)
                materialInput.PermanentProducts = new List<AuxiliarPermanent>();
            foreach (ConsumpterProductDataGrid item in DataGridFinalConsumpterProducts.Items)
            {
                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    Quantity = item.QuantityAdded,
                    SCMEmployeeId = Helper.NameIdentifier,
                };
                materialInput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (PermanentProductDataGrid item in DataGridFinalPermanentProducts.Items)
            {
                AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                {
                    Date = DateTime.Now,
                    ProductId = item.Id,
                    SCMEmployeeId = Helper.NameIdentifier
                };
                materialInput.PermanentProducts.Add(auxiliarPermanent);
            }

            //TASK...
            Task.Run(() => 
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "devolution/add").ToString(), materialInput, Helper.Authentication);
                string message = result.DeserializeJson<string>();
                if (message.Contains("sucesso"))
                {
                    RescueData(workOrder);
                }
                MessageBox.Show(message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void UpdateInput()
        {
            MaterialInput materialInput = this.previousMaterialInput;
            if ((DataGridFinalConsumpterProducts.Items.Count > 0) && (materialInput.ConsumptionProducts == null))
            {
                materialInput.ConsumptionProducts = new List<AuxiliarConsumption>();
            }
            if ((DataGridFinalPermanentProducts.Items.Count > 0) && (materialInput.PermanentProducts == null))
            {
                materialInput.PermanentProducts = new List<AuxiliarPermanent>();
            }

            //var listProduct = materialInput.ConsumptionProducts.ToList();

            foreach (ConsumpterProductDataGrid item in DataGridFinalConsumpterProducts.Items)
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
                    materialInput.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    //listProduct[FinalConsumpterProductInputAdded.IndexOf(item)].Quantity = 
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }

            foreach (PermanentProductDataGrid item in DataGridFinalPermanentProducts.Items)
            {
                //MEXER
                if (!DataGridFinalPermanentProducts.Items.Contains(item))
                {
                    var permanentProduct = materialInput.PermanentProducts.Single(x => x.ProductId == item.Id);
                    materialInput.PermanentProducts.Remove(permanentProduct);
                }

                if (!materialInput.PermanentProducts.Any(x => x.ProductId == item.Id))
                {
                    AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                    {
                        Date = DateTime.Now,
                        ProductId = item.Id,
                        SCMEmployeeId = Helper.NameIdentifier
                    };
                    materialInput.PermanentProducts.Add(auxiliarPermanent);
                }
            }

            var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"devolution/Update/{materialInput.Id}").ToString(), this.previousMaterialInput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = ((FrameworkElement)sender);
            var product = ((FrameworkElement)sender).DataContext as ConsumpterProductDataGrid;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);
            if (dialog.ShowDialog() == true)
            {

                //if (button.Name != "BtnToAdd")
                //    productInConsumpterProductInput = ConsumpterProductInput.Single(x => x == product);
                product.QuantityAdded = dialog.QuantityAdded;
                //int index = ProductToAddDataGrid.SelectedIndex;
                this.DataGridConsumpterProducts.Items.Refresh();
                this.DataGridFinalConsumpterProducts.Items.Refresh();
                if (!this.DataGridFinalConsumpterProducts.Items.Contains(product))
                {
                    if (product.QuantityAdded == 0)
                        return;
                    product.NewProduct = button.Name == "BtnToAdd";
                    this.DataGridFinalConsumpterProducts.Items.Add(product);
                    //FinalConsumpterProductInputAdded.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                    {
                        //this.previousMaterialInput.ConsumptionProducts.Remove(product.ConsumptionProduct);
                        //ConsumpterProductInput.Remove(product);
                        this.DataGridFinalConsumpterProducts.Items.Remove(product);
                    }
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                    }
                    product.ProductChanged = true;
                }
                this.DataGridConsumpterProducts.UnselectAll();
                this.DataGridFinalConsumpterProducts.UnselectAll();
                MenuItemEventHandler?.Invoke(3, (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
                this.ButtonFinish.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
                this.ButtonNext3.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            }
        }

        private void ConsumpterProductSearch(string query)
        {
            if (query == string.Empty)
                return;

            query = System.Uri.EscapeDataString(query);
            //Limpa datagrid de consumo
            this.DataGridConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridConsumpterProducts.Items.Clear(); DataGridConsumpterProducts.UnselectAll(); }));
            

            Uri uriProductsSearch = new Uri(Helper.ServerAPI, $"generalproduct/search/{query}");

            //Requisição de dados baseado na busca
            var products = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);

            foreach (var infoProduct in products.ToList())
            {
                double productInputQuantity = 0.00d;
                try
                {
                    var infoInput = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.ServerAPI, $"input/workorder/{query}").ToString(), Helper.Authentication);
                    var productInput = infoInput.ConsumptionProducts.First(x => x.ProductId == infoProduct.Id);
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
                    Id = infoProduct.Id,
                    Quantity = infoProduct.Stock,
                    QuantityOutput = infoProduct.Stock,
                    QuantityAdded = productInputQuantity,
                    NewProduct = false,
                };

                this.DataGridConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridConsumpterProducts.Items.Add(consumpterProductDataGrid); }));
            }
        }

        private void PermanentProductSearch(string query)
        {
            if (query == string.Empty)
                return;

            query = System.Uri.EscapeDataString(query);

            Uri uriProductsSearch = new Uri(Helper.ServerAPI, $"PermanentProduct/search/{query}");

            this.DataGridPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridPermanentProducts.Items.Clear(); }));

            var products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication).Where(x => !string.IsNullOrWhiteSpace(x.WorkOrder));
            foreach (var product in products)
            {
                var information = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{product.InformationProduct}").ToString(), Helper.Authentication);

                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
                {
                    Id = product.Id,
                    Code = information.Code,
                    Description = information.Description,
                    Patrimony = product.Patrimony,
                    Quantity = information.Stock,
                    NewProduct = true,
                };
                this.DataGridPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridPermanentProducts.Items.Add(permanentProductDataGrid); }));
            }

        }

        string previousWorkOrder = string.Empty;
        bool DataGridConsumpterProductFocus = false;
        bool DataGridPermanentProductFocus = false;

        private void BtnAddRemovePermanent_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as PermanentProductDataGrid;
            if (product.BtnContent == "Adicionar")
            {
                this.DataGridFinalPermanentProducts.Items.Add(product);
                product.QuantityAdded += 1;
                //product.BtnContent = "Remover";
            }
            else
            {
                //product.BtnContent = "Adicionar";
                product.QuantityAdded -= 1;
                this.DataGridFinalPermanentProducts.Items.Remove(product);
            }
            this.DataGridPermanentProducts.Items.Refresh();
            this.DataGridFinalPermanentProducts.Items.Refresh();
            this.DataGridPermanentProducts.UnselectAll();
            this.DataGridFinalPermanentProducts.UnselectAll();

            MenuItemEventHandler?.Invoke(3, (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
            this.ButtonFinish.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            this.ButtonNext3.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
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
                //MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ButtonNext1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void TxtSearchConsumpterProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearchConsumpterProduct.Text;
                Task.Run(() => ConsumpterProductSearch(query));
            }
        }

        private void ButtonSearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchConsumpterProduct.Text;
            Task.Run(() => ConsumpterProductSearch(query));
        }

        private void DataGridConsumpterProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
            else
            {
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        private void ButtonPrevious1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[0];
        }

        private void ButtonNext2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }

        private void TxtSearchPermanentProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearchPermanentProduct.Text;
                Task.Run(() => { PermanentProductSearch(query); });
            }
        }

        private void ButtonSearchPermanentProduct_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchPermanentProduct.Text;
            Task.Run(() => { PermanentProductSearch(query); });
        }

        private void DataGridPermanentProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
            else
            {
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        private void ScrollViewerFinish_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;

            var dtFinalConsumpter = DataGridFinalConsumpterProducts;
            var dtFinalPermanent = DataGridFinalPermanentProducts;

            var scrollViewerFinalConsumpter = dtFinalConsumpter.GetScrollViewer();
            var scrollViewerFinalPermanent = dtFinalPermanent.GetScrollViewer();

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (DataGridConsumpterProductFocus || DataGridPermanentProductFocus)
                {
                    return;
                }
            }

            if (e.Delta < 0) //Para baixo
            {
                if (((scrollViewerFinalConsumpter.VerticalOffset == scrollViewerFinalConsumpter.ScrollableHeight)))
                {
                    scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
                if ((scrollViewerFinalPermanent.VerticalOffset == scrollViewerFinalPermanent.ScrollableHeight) && DataGridPermanentProductFocus)
                {
                    scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
            else if (e.Delta > 0) //Para cima
            {
                if ((scrollViewerFinalConsumpter.VerticalOffset == 0) && DataGridConsumpterProductFocus)
                {
                    scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
                if ((scrollViewerFinalPermanent.VerticalOffset == 0) && DataGridPermanentProductFocus)
                {
                    scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        private void DataGridFinalConsumpterProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
            else
            {
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        private void DataGridFinalConsumpterProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            DataGridConsumpterProductFocus = true;
        }

        private void DataGridFinalConsumpterProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGridConsumpterProductFocus = false;
        }

        private void TextBoxWorkOrder_KeyUp(object sender, KeyEventArgs e)
        {
            var workOrder = TextBoxWorkOrder.Text;
            if (previousWorkOrder == workOrder)
                return;
            Task.Run(() =>
            {
                bool existMonitoring = false;
                try
                {
                    existMonitoring = Monitoring.ExistsMonitoring(workOrder);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.PackIconStatus.Visibility = Visibility.Visible;
                    });
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.PackIconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CC0000");
                        this.PackIconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                        this.PackIconStatus.ToolTip = "Campo vazio";

                        this.ButtonFinish.IsEnabled = false;
                    });
                    return;
                }

                if (existMonitoring)
                {
                    //Ordem de serviço existente
                    bool checkWorkOrder = Monitoring.CheckWorkOrder(workOrder);
                    if (checkWorkOrder) //Ordem de serviço fechada
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.PackIconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#CC0000");
                            this.PackIconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                            this.PackIconStatus.ToolTip = "Ordem de serviço fechada";
                            RescueData(workOrder);
                            InputData(false, false, false, false);
                            this.ButtonFinish.IsEnabled = false;
                        });
                    }
                    else //Ordem de serviço aberta
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.PackIconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("DarkOrange");
                            this.PackIconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                            this.PackIconStatus.ToolTip = "Ordem de serviço aberta";
                            RescueData(workOrder);
                            this.ButtonFinish.IsEnabled = true;
                        });
                    }
                }
                else
                {
                    //Ordem de serviço inexistente
                    this.Dispatcher.Invoke(() =>
                    {
                        this.PackIconStatus.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("Green");
                        this.PackIconStatus.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
                        this.PackIconStatus.ToolTip = "Ordem de serviço disponível";
                        ClearData();
                        InputData(true, true, true, true);

                        this.ButtonFinish.IsEnabled = true;
                    });
                }
                this.Dispatcher.Invoke(() =>
                {
                    this.ButtonNext1.IsEnabled = AllowNext1();
                    this.ButtonNext3.IsEnabled = AllowNext1();
                    MenuItemEventHandler?.Invoke(1, AllowNext1());
                    MenuItemEventHandler?.Invoke(2, AllowNext1());
                    MenuItemEventHandler?.Invoke(3, AllowNext1());
                });
            });
            previousWorkOrder = workOrder;
        }

        private bool AllowNext1()
        {
            bool WO = this.TextBoxWorkOrder.Text != string.Empty;
            bool DateWO = this.DatePickerMovingDate.SelectedDate != null;
            bool ServiceLocalization = this.TextBoxServiceLocalization.Text != string.Empty;
            bool TextBoxApplicant = this.TextBoxApplicant.Text != string.Empty;
            bool ComboBoxReference = this.ComboBoxReference.Text != string.Empty;

            return WO && DateWO && ServiceLocalization && TextBoxApplicant && ComboBoxReference;
        }

        private void DatePickerMovingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
            MenuItemEventHandler?.Invoke(1, AllowNext1());
            MenuItemEventHandler?.Invoke(2, AllowNext1());
        }

        private void TextBoxServiceLocalization_KeyUp(object sender, KeyEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
        }

        private void TextBoxRegisterApplicant_KeyUp(object sender, KeyEventArgs e)
        {
            string register = this.TextBoxRegisterApplicant.Text;
            Task.Run(() =>
            {
                GetName(register);
                this.Dispatcher.Invoke(() =>
                {
                    this.ButtonNext1.IsEnabled = AllowNext1();
                    MenuItemEventHandler?.Invoke(1, AllowNext1());
                    MenuItemEventHandler?.Invoke(2, AllowNext1());
                });
            });
        }

        private void TextBoxApplicant_KeyUp(object sender, KeyEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
        }

        private void ButtonPrevious2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void ButtonNext3_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[3];
        }

        private void DataGridFinalPermanentProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
            else
            {
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        private void DataGridFinalPermanentProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            DataGridPermanentProductFocus = true;
        }

        private void DataGridFinalPermanentProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGridPermanentProductFocus = false;
        }

        private void ButtonPrevious3_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = this.TextBoxWorkOrder.Text;
            DateTime dateTime = (DatePickerMovingDate.DisplayDate == DateTime.Today) ? (DateTime.Now) : DatePickerMovingDate.DisplayDate;
            string register = TextBoxRegisterApplicant.Text;
            string serviceLocation = TextBoxServiceLocalization.Text;
            string numberSector = TextBoxWorkOrder.Text.Substring(0, 2);
            Regarding regarding = ((Regarding)(ComboBoxReference.SelectedIndex + 1));
            Task.Run(() =>
            {
                if (Monitoring.ExistsMonitoring(workOrder))
                {
                    var existsInput = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"devolution/existsinput/{System.Uri.EscapeDataString(workOrder)}").ToString(), Helper.Authentication);
                    if (existsInput)
                    {
                        UpdateInput();
                    }
                    else
                    {
                        AddInput(regarding, dateTime, workOrder);
                    }
                }
                else
                {
                    try
                    {
                        AddMonitoring(dateTime, register, numberSector, workOrder, serviceLocation);
                        AddInput(regarding, dateTime, workOrder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ocorreu um erro durante o cadastro da ordem de serviço", MessageBoxButton.OK, MessageBoxImage.Error);
                        RemoveMonitoring(workOrder);
                    }
                }
            });
        }

        private void RemoveMonitoring(string workOrder)
        {
            workOrder = System.Uri.EscapeDataString(workOrder);

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
            var result = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"Monitoring/RemoveByWorkOrder/{workOrder}").ToString(), Helper.Authentication);
            MessageBox.Show(result.DeserializeJson<string>(), "Finalização", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            SCM2020___Client.Templates.Movement.MaterialMovement movement = new Templates.Movement.MaterialMovement(PrincipalMonitoring.Work_Order);
            PrintORExport = true;
            Document = movement.RenderizeHtml();
            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.WebBrowser.NavigateToString(Document);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            SCM2020___Client.Templates.Movement.MaterialMovement movement = new Templates.Movement.MaterialMovement(PrincipalMonitoring.Work_Order);

            PrintORExport = false;
            Document = movement.RenderizeHtml();
            this.WebBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.WebBrowser.NavigateToString(Document);
        }
    }
}
