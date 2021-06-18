using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
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
using MaterialDesignThemes.Wpf;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Interação lógica para InputByVendor.xaml
    /// </summary>
    public partial class InputByVendor : UserControl
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
                            ShowConsumptionProducts();
                            break;
                        case "2":
                            ShowPermamentProducts();
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
            this.GridConsumptionProducts.Visibility = Visibility.Collapsed;
            this.GridPermamentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;

        }
        private void ShowConsumptionProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumptionProducts.Visibility = Visibility.Visible;
            this.GridPermamentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }
        private void ShowPermamentProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumptionProducts.Visibility = Visibility.Collapsed;
            this.GridPermamentProducts.Visibility = Visibility.Visible;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }
        private void ShowFinish()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridConsumptionProducts.Visibility = Visibility.Collapsed;
            this.GridPermamentProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Visible;
        }

        public InputByVendor()
        {
            InitializeComponent();
            this.DataGridToAddPermamentProduct.ItemsSource = ListFinalAddedPermanentProducts;
            this.DataGridFinalPermanentProducts.ItemsSource = ListFinalAddedPermanentProducts;
            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Informações", 0, true),
                new MenuItem(Name: "Produtos Consumíveis", 1, false),
                new MenuItem(Name: "Produtos Permanentes", 2, false),
                new MenuItem(Name: "Finalização", 3, false)
            };
            CurrentMenuItem = Menu[0];

            Uri vendorUri = new Uri(Helper.ServerAPI, "vendor/");
            var vendors = APIClient.GetData<List<Vendor>>(vendorUri.ToString());
            var nameVendors = vendors.Select(x => x.Name).ToList();
            this.ComboBoxVendor.ItemsSource = nameVendors;
            this.ComboBoxVendor.SelectedIndex = 0;
            this.DatePickerMovingDate.SelectedDate = DateTime.Now.Date;
        }

        #region AllowButtonsNext
        private bool AllowNext1()
        {
            return ((this.TextBoxInvoice.Text.Trim() != string.Empty) && (this.ComboBoxVendor.Items.Count > 0) && (this.DatePickerMovingDate.SelectedDate != null) && (DateTime.TryParse(this.DatePickerMovingDate.SelectedDate.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), out DateTime _)));
        }

        private bool AllowNext2()
        {
            return (DataGridFinalConsumptionProducts.Items.Count > 0);
        }
        #endregion

        private bool existsInput = false;
        private ModelsLibraryCore.MaterialInputByVendor previousInput;
        private List<PermanentProductDataGrid> ListFinalAddedPermanentProducts = new List<PermanentProductDataGrid>();

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearch.Text;
            Task.Run(() => ConsumpterProductSearch(query));
        }
        private void ConsumpterProductSearch(string query)
        {
            if (query == string.Empty)
                return;
            query = System.Uri.EscapeDataString(query);

            this.DataGridToAddProduct.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridToAddProduct.Items.Clear(); }));

            Uri uriProductsSearch = new Uri(Helper.ServerAPI, $"generalproduct/search/{query}");

            //Requisição de dados baseado na busca
            var products = APIClient.GetData<List<ConsumptionProduct>>(uriProductsSearch.ToString(), Helper.Authentication);

            foreach (var item in products)
            {
                ConsumpterProductDataGrid productsToInput = new ConsumpterProductDataGrid()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Description = item.Description,
                    Quantity = item.Stock
                };
                this.DataGridToAddProduct.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridToAddProduct.Items.Add(productsToInput); }));
            }
        }

        private void BtnAddRemove_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as ConsumpterProductDataGrid;
            var dialog = new DialogBox.AddAndRemove(product.QuantityAdded);

            if (dialog.ShowDialog() == true)
            {
                product.QuantityAdded = dialog.QuantityAdded;
                int index = DataGridToAddProduct.SelectedIndex;
                DataGridToAddProduct.Items.Refresh();
                DataGridFinalConsumptionProducts.Items.Refresh();
                if (!DataGridFinalConsumptionProducts.Items.Contains(product))
                {
                    if (product.QuantityAdded == 0)
                        return;
                    product.NewProduct = true;
                    DataGridFinalConsumptionProducts.Items.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                        DataGridFinalConsumptionProducts.Items.Remove(product);
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                        product.ProductChanged = true;

                    }
                }
                DataGridToAddProduct.UnselectAll();
                DataGridFinalConsumptionProducts.UnselectAll();

                this.ButtonNext2.IsEnabled = AllowNext2();
                this.ButtonFinish.IsEnabled = AllowNext2();
                this.MenuItemEventHandler?.Invoke(3, AllowNext2());
            }
        }

        private void AddInput()
        {
            DateTime dateTime = DateTime.Now;
            string invoice = null;
            int vendorId = 0;

            this.DatePickerMovingDate.Dispatcher.Invoke(() => { dateTime = this.DatePickerMovingDate.SelectedDate.Value; });
            this.TextBoxInvoice.Dispatcher.Invoke(() => { invoice = TextBoxInvoice.Text; });
            this.ComboBoxVendor.Dispatcher.Invoke(() => { vendorId = ComboBoxVendor.SelectedIndex + 1; });

            MaterialInputByVendor materialInputByVendor = new MaterialInputByVendor();
            materialInputByVendor.Invoice = invoice;
            materialInputByVendor.MovingDate = dateTime;
            materialInputByVendor.VendorId = vendorId;

            List<AuxiliarConsumption> p = new List<AuxiliarConsumption>();

            foreach (var item in DataGridFinalConsumptionProducts.Items)
            {
                ConsumpterProductDataGrid product = item as ConsumpterProductDataGrid;
                AuxiliarConsumption auxiliarConsumption = new AuxiliarConsumption()
                {
                    Date = DateTime.Now,
                    ProductId = product.Id,
                    Quantity = product.QuantityAdded,
                    SCMEmployeeId = Helper.NameIdentifier
                };
                p.Add(auxiliarConsumption);
            }

            foreach (var inputPermamentProduct in ListFinalAddedPermanentProducts)
            {
                if (inputPermamentProduct.NewProduct)
                {
                    //1- Adicionar material no controller PermanentProduct
                    //2- Receber o ID do produto adicionado
                    //3- Adicionar na movimentação de entrada
                }
                //AuxiliarPermanent auxiliarPermanent = new AuxiliarPermanent()
                //{
                //    Date = materialInputByVendor.MovingDate,
                //    SCMEmployeeId = Helper.NameIdentifier,
                //    ProductId = inputPermamentProduct.Id
                //};
            }

            materialInputByVendor.ConsumptionProducts = p;
            Task.Run(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "input/Add").ToString(), materialInputByVendor, Helper.Authentication);
                string message = result.DeserializeJson<string>();
                if (message.Contains("sucesso"))
                {
                    RescueData(invoice);
                }
                MessageBox.Show(message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void UpdateInput()
        {
            ModelsLibraryCore.MaterialInputByVendor input = this.previousInput;
            if (this.DataGridFinalConsumptionProducts.Items.Count == 0)
            {
                //Pergunta se deseja apagar
                //Não há sentido de manter uma entrada por fornecedor com informações e sem materiais envolvidos
                MessageBoxResult resultDialog = MessageBox.Show("Você deseja apagar a entrada por fornecedor?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultDialog == MessageBoxResult.Yes)
                {
                    var result = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"Input/Remove/{previousInput.Id}").ToString(), Helper.Authentication);
                    MessageBox.Show(result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            foreach (ConsumpterProductDataGrid item in this.DataGridFinalConsumptionProducts.Items)
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
                    input.ConsumptionProducts.Add(auxiliarConsumption);
                }
                if (item.ProductChanged)
                {
                    item.ConsumptionProduct.Quantity = item.QuantityAdded;
                }
            }

            Task.Run(() => 
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"input/update/{input.Id}").ToString(), input, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        string previousInvoice = string.Empty;

        private void RescueData(string invoice)
        {
            if (invoice == string.Empty)
                return;
            invoice = System.Uri.EscapeDataString(invoice);

            //Limpar campos e permitir uso
            ClearData();
            InputData(true);

            previousInput = null;
            try
            {
                previousInput = APIClient.GetData<ModelsLibraryCore.MaterialInputByVendor>(new Uri(Helper.ServerAPI, $"input/invoice/{invoice}").ToString(), Helper.Authentication);
                existsInput = true;
            }
            catch (HttpRequestException) //Entrada por fornecedor inexistente
            {
                //Limpar campos e permitir uso
                ClearData();
                InputData(true);
                existsInput = false;
                this.Dispatcher.Invoke(() => 
                {
                    this.ButtonNext2.IsEnabled = false;
                    this.ButtonFinish.IsEnabled = false;
                    this.MenuItemEventHandler?.Invoke(3, false);
                });
                return;

            }
            //Bloquear combobox e data
            InputData(false);
            //Colocando informações
            this.ComboBoxVendor.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxVendor.SelectedIndex = previousInput.VendorId - 1; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = previousInput.MovingDate; }));
            //Preencher datagrid
            foreach (var item in previousInput.ConsumptionProducts)
            {
                ModelsLibraryCore.ConsumptionProduct information = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{item.ProductId}").ToString(), Helper.Authentication);
                ConsumpterProductDataGrid product = new ConsumpterProductDataGrid()
                {
                    Id = item.ProductId,
                    Code = information.Code,
                    Description = information.Description,
                    QuantityAdded = item.Quantity,
                    Quantity = information.Stock,
                    ConsumptionProduct = item
                };
                this.DataGridFinalConsumptionProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumptionProducts.Items.Add(product); }));
            }
            this.Dispatcher.Invoke(() => 
            {
                this.ButtonNext2.IsEnabled = AllowNext1();
                this.MenuItemEventHandler?.Invoke(3, AllowNext1());
                this.ButtonFinish.IsEnabled = AllowNext1();
            });
        }

        private void ClearData()
        {
            this.ComboBoxVendor.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxVendor.SelectedIndex = 0; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = DateTime.Now; }));
            this.DataGridFinalConsumptionProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridFinalConsumptionProducts.Items.Clear(); }));
        }
        private void InputData(bool IsEnable)
        {
            this.ComboBoxVendor.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxVendor.IsEnabled = IsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = IsEnable; }));
        }

        private void ButtonNext1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }
        private void ButtonNext2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string query = TxtSearch.Text;
                Task.Run(() => ConsumpterProductSearch(query));
            }
        }

        private void DataGridToAddProduct_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
        }


        private void DataGridAddedProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
        }
        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            string invoice = this.TextBoxInvoice.Text;
            Task.Run(() => 
            {
                FinishMovement(invoice);
            });
        }

        private void FinishMovement(string invoice)
        {
            invoice = System.Uri.EscapeDataString(invoice);
            bool allReady = false;
            this.Dispatcher.Invoke(() => { allReady = AllowNext1() && AllowNext2(); });
            if (allReady)
            {
                var existsPreviousInput = APIClient.GetData<bool>(new Uri(Helper.ServerAPI, $"input/ExistsInput/{invoice}").ToString(), Helper.Authentication);

                if (existsPreviousInput)
                    UpdateInput();
                else
                    AddInput();
            }
        }
        private void FillSummary()
        {
            string invoice = null, vendor = null, strDate = null;

            this.Dispatcher.Invoke(() => 
            {
                invoice = this.TextBoxInvoice.Text;
                vendor = this.ComboBoxVendor.Text;
                strDate = this.DatePickerMovingDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            });

            List<SummaryInfo> infos = new List<SummaryInfo>()
            {
                new SummaryInfo("DOC/SM/OS", invoice, PackIconKind.Invoice),
                new SummaryInfo("Fornecedor", vendor, PackIconKind.Truck), //PackIconKind.Truck
                new SummaryInfo("Data de Movimentação", strDate, PackIconKind.CalendarToday), //event
            };
            this.ListView.ItemsSource = infos;
        }



        private void TextBoxInvoice_KeyUp(object sender, KeyEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
            this.MenuItemEventHandler?.Invoke(1, AllowNext1());
            this.MenuItemEventHandler?.Invoke(2, AllowNext1());
            if (AllowNext1())
                FillSummary();

            var invoice = TextBoxInvoice.Text;
            if (previousInvoice == invoice)
                return;
            Task.Run(() => RescueData(invoice));
            previousInvoice = invoice;
        }

        private void ButtonPrevious1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[0];
        }

        private void ButtonPrevious2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void DatePickerMovingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
            this.MenuItemEventHandler?.Invoke(1, AllowNext1());
            this.MenuItemEventHandler?.Invoke(2, AllowNext1());
            if (AllowNext1())
                FillSummary();
        }

        private void ComboBoxVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllowNext1())
                FillSummary();
        }


        private void ButtonPrevious3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddPermanentProduct_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window()
            {
                Title = "Adicionar Novo Produto Permanente - Sistema de Controle de Materiais",
            };
            var permanentProduct = new Register.PermanentProduct(window);
            window.Content = permanentProduct;
            window.ShowDialog();
            if (permanentProduct.Permanent_Product != null)
            {
                PermanentProductDataGrid permanentProductDataGrid = new PermanentProductDataGrid() 
                {
                    Code = permanentProduct.Consumption_Product.Code,
                    Id = permanentProduct.Consumption_Product.Id,
                    Description = permanentProduct.Consumption_Product.Description,
                    NewProduct = true,
                    Patrimony = permanentProduct.Permanent_Product.Patrimony,
                };
                permanentProductDataGrid.QuantityAdded += 1;
                ListFinalAddedPermanentProducts.Add(permanentProductDataGrid);
                this.DataGridToAddPermamentProduct.Items.Refresh();
                this.DataGridFinalPermanentProducts.Items.Refresh();
                this.DataGridToAddPermamentProduct.UnselectAll();
                this.DataGridFinalPermanentProducts.UnselectAll();

                MenuItemEventHandler?.Invoke(3, (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
                this.ButtonFinish.IsEnabled = (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
                this.ButtonNext3.IsEnabled = (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as PermanentProductDataGrid;
            product.QuantityAdded -= 1;
            product.NewProduct = false;
            ListFinalAddedPermanentProducts.Remove(product);

            this.DataGridToAddPermamentProduct.Items.Refresh();
            this.DataGridFinalPermanentProducts.Items.Refresh();
            this.DataGridToAddPermamentProduct.UnselectAll();
            this.DataGridFinalPermanentProducts.UnselectAll();

            MenuItemEventHandler?.Invoke(3, (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0));
            this.ButtonFinish.IsEnabled = (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
            this.ButtonNext3.IsEnabled = (DataGridFinalPermanentProducts.Items.Count > 0) || (DataGridFinalPermanentProducts.Items.Count > 0);
        }

        private void ScrollViewerFinish_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGridFinalConsumptionProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void DataGridFinalConsumptionProducts_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void DataGridFinalPermanentProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGridFinalPermanentProducts_PreviewMouseMove(object sender, MouseEventArgs e)
        {

        }

        private void DataGridFinalPermanentProducts_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
