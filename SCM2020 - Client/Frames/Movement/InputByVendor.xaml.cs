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
                            ShowProducts();
                            break;
                        case "2":
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
            this.GridProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;

        }
        private void ShowProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridProducts.Visibility = Visibility.Visible;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }
        private void ShowFinish()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Visible;
        }

        public InputByVendor()
        {
            InitializeComponent();

            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Informações", 0, true),
                new MenuItem(Name: "Produtos", 1, false),
                new MenuItem(Name: "Finalização", 2, false)
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
            return (DataGridAddedProducts.Items.Count > 0);
        }
        #endregion

        private bool existsInput = false;
        private ModelsLibraryCore.MaterialInputByVendor previousInput;
        private List<ConsumpterProductDataGrid> ListFinalAddedProducts = new List<ConsumpterProductDataGrid>();

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
                DataGridAddedProducts.Items.Refresh();
                if (!DataGridAddedProducts.Items.Contains(product))
                {
                    product.NewProduct = true;
                    DataGridAddedProducts.Items.Add(product);
                }
                else
                {
                    if (dialog.QuantityAdded == 0)
                        DataGridAddedProducts.Items.Remove(product);
                    else
                    {
                        product.QuantityAdded = dialog.QuantityAdded;
                        product.ProductChanged = true;

                    }
                }
                DataGridToAddProduct.UnselectAll();
                DataGridAddedProducts.UnselectAll();

                this.ButtonNext2.IsEnabled = AllowNext2();
                this.MenuItemEventHandler?.Invoke(2, AllowNext2());
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

            foreach (var item in DataGridAddedProducts.Items)
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
            materialInputByVendor.ConsumptionProducts = p;
            Task.Run(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "input/Add").ToString(), materialInputByVendor, Helper.Authentication);
                MessageBox.Show(result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
        private void UpdateInput()
        {
            ModelsLibraryCore.MaterialInputByVendor input = this.previousInput;
            if (this.DataGridAddedProducts.Items.Count == 0)
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

            foreach (ConsumpterProductDataGrid item in this.DataGridAddedProducts.Items)
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

            this.DataGridAddedProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridAddedProducts.Items.Clear(); }));

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
                this.DataGridAddedProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridAddedProducts.Items.Add(product); }));
            }
        }

        private void ClearData()
        {
            this.ComboBoxVendor.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxVendor.SelectedIndex = 0; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.SelectedDate = DateTime.Now; }));
            this.DataGridAddedProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DataGridAddedProducts.Items.Clear(); }));
        }
        private void InputData(bool IsEnable)
        {
            this.ComboBoxVendor.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ComboBoxVendor.IsEnabled = IsEnable; }));
            this.DatePickerMovingDate.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { DatePickerMovingDate.IsEnabled = IsEnable; }));
        }

        private void TextBoxInvoice_LostFocus(object sender, RoutedEventArgs e)
        {
            var invoice = TextBoxInvoice.Text;
            if (previousInvoice == invoice)
                return;
            Task.Run(() => RescueData(invoice));
            previousInvoice = invoice;
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

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxInvoice_KeyUp(object sender, KeyEventArgs e)
        {
            this.ButtonNext1.IsEnabled = AllowNext1();
            MenuItemEventHandler?.Invoke(1, AllowNext1());
            if (AllowNext1())
                FillSummary();
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
            MenuItemEventHandler?.Invoke(1, AllowNext1());
            if (AllowNext1())
                FillSummary();
        }

        private void ComboBoxVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllowNext1())
                FillSummary();
        }
    }
}
