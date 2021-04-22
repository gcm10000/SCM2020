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
using Monitoring = SCM2020___Client.Models.Monitoring;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.Frames.Query;
using WebAssemblyLibrary;
using MaterialDesignThemes.Wpf;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Interação lógica para InputByVendor.xaml
    /// </summary>
    public partial class MaterialOutput : UserControl
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

        class ProductToOutput
        {
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

            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Informações", 0, true),
                new MenuItem(Name: "Produtos Consumíveis", 1, false),
                new MenuItem(Name: "Produtos Permanentes", 2, false),
                new MenuItem(Name: "Finalização", 3, false)
            };
            CurrentMenuItem = Menu[0];
            this.ComboBoxOutputType.SelectedIndex = 0;
            this.DatePickerMovingDate.SelectedDate = DateTime.Now;

        }

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

            this.DataGridPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridPermanentProducts.ItemsSource = null; }));

            var products = APIClient.GetData<List<PermanentProduct>>(uriProductsSearch.ToString(), Helper.Authentication);
            List<PermanentProductDataGrid> permanentProducts = new List<PermanentProductDataGrid>();
            
            foreach (var permanentProduct in products)
            {
                permanentProducts.Add(new PermanentProductDataGrid(new SearchPermanentProduct(permanentProduct)));
            }
            this.DataGridPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridPermanentProducts.ItemsSource = permanentProducts; }));
        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        List<ProductToOutput> FinalConsumpterProductsAdded = new List<ProductToOutput>();
        List<ProductToOutput> FinalPermanentProductsAdded = new List<ProductToOutput>();

        //Método conveniente somente para produtos consumíveis
        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var button = ((FrameworkElement)sender);
            var product = ((FrameworkElement)sender).DataContext as ProductToOutput;
            var dialog = new SCM2020___Client.Frames.DialogBox.AddAndRemove(product.QuantityAdded);
            dialog.Owner = Application.Current.MainWindow;
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
                MenuItemEventHandler?.Invoke(3, (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
                this.ButtonFinish.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
                this.ButtonNext3.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            }
        }
        private void ProductToAddDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            var currentRowIndex = DataGridConsumpterProducts.Items.IndexOf(DataGridConsumpterProducts.CurrentItem);
            MessageBox.Show(currentRowIndex.ToString());
        }

        private void AddOutput(DateTime dateTime, string WorkOrder)
        {
            //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO
            var materialOutput = new ModelsLibraryCore.MaterialOutput
            {
                WorkOrder = WorkOrder,
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
                    Date = DateTime.Now,
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
                //CRIANDO REGISTRO DE UMA NOVA SAÍDA NA ORDEM DE SERVIÇO
                var result2 = APIClient.PostData(new Uri(Helper.ServerAPI, "Output/Add").ToString(), materialOutput, Helper.Authentication);
                MessageBox.Show(result2.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);

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
                //Se no DataGrid de produtos permanentes finais não conter o produto, o material será removido da lista
                if (!DataGridFinalPermanentProducts.Items.Contains(item))
                {
                    var permanentProduct = materialOutput.PermanentProducts.Single(x => x.ProductId == item.Id);
                    materialOutput.PermanentProducts.Remove(permanentProduct);
                }

                //Se na lista de produtos permanentes da movimentação de saída não conter o produto permanente desejado, então trate de adicionar.
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

            MenuItemEventHandler?.Invoke(3, (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
            this.ButtonFinish.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            this.ButtonNext3.IsEnabled = (DataGridFinalConsumpterProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);

        }

        private void ButtonSearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
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
        private void ButtonSearchPermanentProduct_Click(object sender, RoutedEventArgs e)
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

        string previousWorkOrder = string.Empty;
        
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
            Monitoring monitoring = APIClient.GetData<Monitoring>(new Uri(Helper.ServerAPI, $"Monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);

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
                this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DatePickerMovingDate.SelectedDate = monitoring.MovingDate; this.DatePickerMovingDate.DisplayDate = monitoring.MovingDate; }));
                this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxServiceLocalization.Text = monitoring.ServiceLocation; }));

                this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxRegisterApplicant.Text = InfoUser.Register; }));
                this.TextBoxApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.TextBoxApplicant.Text = InfoUser.Name; }));
                EnableInputData(false);

                var existsOutput = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"Output/ExistsOutputByWorkOrder/{workOrder}").ToString(), Helper.Authentication);
                if (existsOutput)
                {
                    var materialOutput = APIClient.GetData<ModelsLibraryCore.MaterialOutput>(new Uri(Helper.ServerAPI, $"Output/workOrder/{workOrder}").ToString(), Helper.Authentication);
                    this.previousMaterialOutput = materialOutput;

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
                        this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumpterProducts.Items.Add(productToOutput); }));
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
                        this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridFinalPermanentProducts.Items.Add(productDataGrid); }));
                        FinalPermanentProductsAdded.Add(productDataGrid);
                    }
                }

                this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));
                this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
            }
            catch (System.Net.Http.HttpRequestException) //Não existe saída de material nesta ordem de serviço.
            {
                EnableInputData(true);
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
            //this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = DateTime.Now; }));
            this.DataGridFinalConsumpterProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumpterProducts.Items.Clear(); }));
            this.DataGridFinalPermanentProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalPermanentProducts.Items.Clear(); }));
        }

        private void EnableInputData(bool IsEnable)
        {
            this.TextBoxRegisterApplicant.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxRegisterApplicant.IsEnabled = IsEnable; }));
            this.ComboBoxOutputType.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxOutputType.IsEnabled = IsEnable; }));
            this.TextBoxServiceLocalization.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { TextBoxServiceLocalization.IsEnabled = IsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = IsEnable; }));
        }

        bool DataToPrintORExportWasRescued = false;
        bool PrintORExport = false;
        string Document = string.Empty;

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

        private void ButtonPrevious1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[0];
        }

        private void ButtonPrevious2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void ButtonNext1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void ButtonNext2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }

        private void ButtonNext3_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[3];
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
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

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
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

        private bool AllowNext1()
        {
            bool WO = this.TextBoxWorkOrder.Text != string.Empty;
            bool DateWO = this.DatePickerMovingDate.SelectedDate != null;
            bool ServiceLocalization = this.TextBoxServiceLocalization.Text != string.Empty;
            bool TextBoxApplicant = this.TextBoxApplicant.Text != string.Empty;
            bool ComboBoxOutputType = this.ComboBoxOutputType.Text != string.Empty;

            return WO && DateWO && ServiceLocalization && TextBoxApplicant && ComboBoxOutputType;
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
                            EnableInputData(true);
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
                        EnableInputData(true);

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

        private void DatePickerMovingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
            MenuItemEventHandler?.Invoke(1, AllowNext1());
            MenuItemEventHandler?.Invoke(2, AllowNext1());
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

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = this.TextBoxWorkOrder.Text;
            DateTime dateTime = (DatePickerMovingDate.DisplayDate == DateTime.Today) ? (DateTime.Now) : DatePickerMovingDate.DisplayDate;
            string register = TextBoxRegisterApplicant.Text;
            string serviceLocation = TextBoxServiceLocalization.Text;
            string numberSector = TextBoxWorkOrder.Text.Substring(0, 2);
            Task.Run(() =>
            {
                if (Monitoring.ExistsMonitoring(workOrder))
                {
                    var existsOutput = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"Output/ExistsOutputByWorkOrder/{System.Uri.EscapeDataString(workOrder)}").ToString(), Helper.Authentication);
                    if (existsOutput)
                    {
                        UpdateOutput();
                    }
                    else
                    {
                        AddOutput(dateTime, workOrder);
                    }
                }
                else
                {
                    try
                    {
                        AddMonitoring(dateTime, register, numberSector, workOrder, serviceLocation);
                        AddOutput(dateTime, workOrder);
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

        private void AddMonitoring(DateTime dateTime, string register, string numberSector, string workOrder, string serviceLocation)
        {

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

        private void TextBoxServiceLocalization_KeyUp(object sender, KeyEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
        }

        private void ButtonPrevious3_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }

        bool DataGridConsumpterProductFocus = false;
        bool DataGridPermanentProductFocus = false;

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

        private void DataGridFinalConsumpterProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            DataGridConsumpterProductFocus = true;
        }

        private void DataGridFinalConsumpterProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGridConsumpterProductFocus = false;
        }

        private void DataGridFinalPermanentProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            DataGridPermanentProductFocus = true;
        }

        private void DataGridFinalPermanentProducts_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGridPermanentProductFocus = false;
        }
    }
}
