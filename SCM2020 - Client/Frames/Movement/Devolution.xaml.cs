using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.Frames.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        private void RescueData(string workOrder)
        {
            workOrder = System.Uri.EscapeDataString(workOrder);
            var uriRequest = new Uri(Helper.Server, $"monitoring/WorkOrder/{workOrder}");
            Monitoring resultMonitoring = null;
            previousDevolutionExists = false;
            try
            {
                resultMonitoring = APIClient.GetData<Monitoring>(uriRequest.ToString(), Helper.Authentication);
                var userId = resultMonitoring.EmployeeId;
                var infoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{userId}").ToString(), Helper.Authentication);
                this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalConsumpterProductsAddedDataGrid.Items.Clear(); }));
                this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalPermanentProductsAddedDataGrid.Items.Clear(); }));
                this.RegisterApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicantTextBox.Text = infoUser.Register; }));
                this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicantTextBox.Text = infoUser.Name; }));
                this.OSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSDatePicker.SelectedDate = resultMonitoring.MovingDate; this.OSDatePicker.DisplayDate = resultMonitoring.MovingDate; }));
                this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = resultMonitoring.ServiceLocation; }));
                //this.RegisterApplicantTextBox.Text = infoUser.Register;
                //this.ApplicantTextBox.Text = infoUser.Name;
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
                previousDevolutionExists = true;
                if (resultMonitoring.Situation == false) //Se a ordem de serviço encontra-se aberta
                {
                    
                    this.ButtonInformation.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonInformation.IsHitTestVisible = false; }));
                    this.ButtonPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPermanentProducts.IsHitTestVisible = true; }));
                    this.ButtonFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonFinish.IsHitTestVisible = true; }));

                    this.InfoScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoScrollViewer.Visibility = Visibility.Visible; }));
                    this.InfoDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoDockPanel.Visibility = Visibility.Visible; }));
                    this.FinalProductsDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalProductsDockPanel.Visibility = Visibility.Collapsed; }));
                    this.PermanentDockPanel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.PermanentDockPanel.Visibility = Visibility.Collapsed; }));

                    try
                    {
                        previousMaterialInput = APIClient.GetData<MaterialInput>(new Uri(Helper.Server, $"devolution/workorder/{workOrder}").ToString(), Helper.Authentication);
                        this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ReferenceComboBox.SelectedIndex = ((int)previousMaterialInput.Regarding) - 1; }));

                        RescueProducts(previousMaterialInput);
                        InputData(false, false);
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
                    this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { FinalConsumpterProductsAddedDataGrid.Items.Clear(); }));
                    this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { FinalPermanentProductsAddedDataGrid.Items.Clear(); }));
                }
            }
        }

        private void ClearData()
        {
            this.RegisterApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { RegisterApplicantTextBox.Text = string.Empty; }));
            this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ApplicantTextBox.Text = string.Empty; }));
            this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ReferenceComboBox.SelectedIndex = 0; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ServiceLocalizationTextBox.Text = string.Empty; }));
            this.OSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { OSDatePicker.SelectedDate = DateTime.Now; }));
        }

        private void InputData(bool IsEnable, bool OSDatePickerIsEnable)
        {
            this.RegisterApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { RegisterApplicantTextBox.IsEnabled = IsEnable; }));
            //this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ApplicantTextBox.IsEnabled = IsEnable; }));
            this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ReferenceComboBox.IsEnabled = IsEnable; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ServiceLocalizationTextBox.IsEnabled = IsEnable; }));
            this.OSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { OSDatePicker.IsEnabled = OSDatePickerIsEnable; }));
        }
        private void InputData(bool ReferenceComboBoxIsEnable, bool OSDatePickerIsEnable, bool RegisterApplicantTextBoxIsEnable, bool ServiceLocalizationIsEnable)
        {
            this.RegisterApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { RegisterApplicantTextBox.IsEnabled = RegisterApplicantTextBoxIsEnable; }));
            this.ReferenceComboBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ReferenceComboBox.IsEnabled = ReferenceComboBoxIsEnable; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ServiceLocalizationTextBox.IsEnabled = ServiceLocalizationIsEnable; }));
            this.OSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { OSDatePicker.IsEnabled = OSDatePickerIsEnable; }));
        }

        private void RescueProducts(ModelsLibraryCore.MaterialInput materialInput)
        {
            this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalConsumpterProductsAddedDataGrid.Items.Clear(); } ));
            //this.FinalConsumpterProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { } ));
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
                if (item.Quantity != 0)
                    this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalConsumpterProductsAddedDataGrid.Items.Add(consumpterProductDataGrid); }));

                ConsumpterProductInput.Add(consumpterProductDataGrid);
            }

            this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalPermanentProductsAddedDataGrid.Items.Clear(); }));

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
                this.FinalPermanentProductsAddedDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.FinalPermanentProductsAddedDataGrid.Items.Add(consumpterProductDataGrid); }));
            }
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
            DateTime dateTime = (OSDatePicker.DisplayDate == DateTime.Today) ? (DateTime.Now) : OSDatePicker.DisplayDate;

            var register = RegisterApplicantTextBox.Text;
            string userId = string.Empty;
            try
            {
                userId = APIClient.GetData<string>(new Uri(Helper.Server, $"User/UserId/{register}").ToString());
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
            Monitoring monitoring = new Monitoring()
            {
                SCMEmployeeId = Helper.NameIdentifier,
                Situation = false,
                ClosingDate = null,
                EmployeeId = userId,
                //O setor é referente ao funcionário que solicitou material na ordem de serviço
                RequestingSector = Helper.CurrentSector.Id,
                MovingDate = dateTime,
                Work_Order = OSTextBox.Text,
                ServiceLocation = ServiceLocalizationTextBox.Text
            };

            MaterialInput materialInput = new MaterialInput()
            {
                Regarding = ((Regarding)(ReferenceComboBox.SelectedIndex + 1)),
                WorkOrder = OSTextBox.Text,
                MovingDate = OSDatePicker.SelectedDate.Value,
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
                    SCMEmployeeId = Helper.NameIdentifier,
                };
                materialInput.ConsumptionProducts.Add(auxiliarConsumption);
            }
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
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
                var workOrder = System.Uri.EscapeDataString(monitoring.Work_Order);

                var checkMonitoring = APIClient.GetData<bool>(new Uri(Helper.Server, $"monitoring/checkworkorder/{workOrder}").ToString(), Helper.Authentication);
                if (!checkMonitoring)
                {
                    //CRIANDO REGISTRO NO BANCO DE DADOS DE UMA NOVA ORDEM DE SERVIÇO...
                    var result1 = APIClient.PostData(new Uri(Helper.Server, "Monitoring/Add").ToString(), monitoring, Helper.Authentication);
                    MessageBox.Show(result1.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                var result2 = APIClient.PostData(new Uri(Helper.Server, "devolution/add").ToString(), materialInput, Helper.Authentication);
                MessageBox.Show(result2.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
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
            var listProduct = materialInput.ConsumptionProducts.ToList();
            foreach (ConsumpterProductDataGrid item in ConsumpterProductInput)
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
                    listProduct.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    listProduct[ConsumpterProductInput.IndexOf(item)].Quantity = 
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }
            materialInput.ConsumptionProducts = listProduct;
            foreach (PermanentProductDataGrid item in FinalPermanentProductsAddedDataGrid.Items)
            {
                //MEXER
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

            var result = APIClient.PostData(new Uri(Helper.Server, $"devolution/Update/{materialInput.Id}").ToString(), this.previousMaterialInput, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        List<ConsumpterProductDataGrid> ConsumpterProductInput = new List<ConsumpterProductDataGrid>();
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
                this.ConsumpterProductToAddDataGrid.Items.Refresh();
                this.FinalConsumpterProductsAddedDataGrid.Items.Refresh();
                if (!this.FinalConsumpterProductsAddedDataGrid.Items.Contains(product))
                {
                    product.NewProduct = button.Name == "BtnToAdd";
                    this.FinalConsumpterProductsAddedDataGrid.Items.Add(product);
                    ConsumpterProductInput.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                    {
                        //this.previousMaterialInput.ConsumptionProducts.Remove(product.ConsumptionProduct);
                        //ConsumpterProductInput.Remove(product);
                        this.FinalConsumpterProductsAddedDataGrid.Items.Remove(product);
                    }
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                    }
                    product.ProductChanged = true;
                }
                this.ConsumpterProductToAddDataGrid.UnselectAll();
                this.FinalConsumpterProductsAddedDataGrid.UnselectAll();
            }
        }
        private void PermanentProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        private void TxtProductConsumpterSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string workOrder = TxtProductConsumpterSearch.Text;
                Task.Run(() => ConsumpterProductSearch(workOrder));
            }
        }
        private void ConsumpterProductSearchButton_Click(object sender, RoutedEventArgs e)
        {
                string workOrder = TxtProductConsumpterSearch.Text;
                Task.Run(() => ConsumpterProductSearch(workOrder));
        }

        private void ConsumpterProductSearch(string workOrder)
        {
            if (workOrder == string.Empty)
                return;

            //Limpa datagrid de consumo
            this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ConsumpterProductToAddDataGrid.Items.Clear(); ConsumpterProductToAddDataGrid.UnselectAll(); }));


            Uri uriProductsSearch = new Uri(Helper.Server, $"generalproduct/search/{workOrder}");

            //Requisição de dados baseado na busca
            var products = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString());

            foreach (var infoProduct in products.ToList())
            {
                double productInputQuantity = 0.00d;
                try
                {
                    var infoInput = APIClient.GetData<ModelsLibraryCore.MaterialInput>(new Uri(Helper.Server, $"input/workorder/{workOrder}").ToString(), Helper.Authentication);
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

                this.ConsumpterProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ConsumpterProductToAddDataGrid.Items.Add(consumpterProductDataGrid); }));

            }
        }
        private void TxtPermanentProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = this.TxtPermanentProductSearch.Text;
                Task.Run(() =>
                {
                    SearchPermamentProduct(query);

                });
            }
        }
        private void PermanentProductButton_Click(object sender, RoutedEventArgs e)
        {
            string query = this.TxtPermanentProductSearch.Text;
            Task.Run(() =>
            {
                SearchPermamentProduct(query);

            });
        }

        private void SearchPermamentProduct(string query)
        {
            if (query == string.Empty)
                return;

            Uri uriProductsSearch = new Uri(Helper.Server, $"PermanentProduct/search/{query}");

            this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { PermanentProductToAddDataGrid.Items.Clear(); }));

            List<PermanentProduct> products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
            foreach (var product in products)
            {
                var information = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{product.InformationProduct}").ToString(), Helper.Authentication);

                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid()
                {
                    Id = product.Id,
                    Code = information.Code,
                    Description = information.Description,
                    Patrimony = product.Patrimony,
                    Quantity = information.Stock,
                    NewProduct = true,
                };
                this.PermanentProductToAddDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { PermanentProductToAddDataGrid.Items.Add(permanentProductDataGrid); }));
            }

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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RegisterApplicantTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string register = this.RegisterApplicantTextBox.Text;
            Task.Run(() => GetName(register));
        }

        private void RegisterApplicantTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string register = this.RegisterApplicantTextBox.Text;
                Task.Run(() => GetName(register));
            }
        }
        private void GetName(string register)
        {
            if (string.IsNullOrWhiteSpace(register))
                return;
            try
            {
                //Zera informação
                this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ApplicantTextBox.Text = string.Empty; }));
                //Recebe informações
                var infoUser = APIClient.GetData<InfoUser>(new Uri(Helper.Server, $"user/InfoUserRegister/{register}").ToString(), Helper.Authentication);
                //Atribui o nome do funcionário no campo correspondente
                this.ApplicantTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ApplicantTextBox.Text = infoUser.Name; }));
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Não existe um funcionário com esta matrícula.", "Matrícula sem funcionário", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
