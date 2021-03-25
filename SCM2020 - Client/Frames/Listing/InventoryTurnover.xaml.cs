using ModelsLibraryCore.RequestingClient;
using MSHTML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para InventoryTurnover.xam
    /// </summary>
    public partial class InventoryTurnover : UserControl
    {
        // INVENTÁRIO ROTATIVO EXIBE:
        // CÓDIGO
        // DESCRIÇÃO
        // QUANTIDADE
        // DE PRODUTOS SELECIONADOS
        // FILTRAR PRODUTO POR CÓDIGO OU DESCRIÇÃO
        // MULTIPLE CONTAINS POR PALAVRAS .Split(' ')

        // UTILIZAR WEBBROWSER POR TER UMA MELHOR PERFORMANCE 
        // NO TRATAMENTO DE VISUALIZAÇÃO DE DADOS
        // DO QUE DATAGRID

        // COMO QUE OS DADOS DO INVENTÁRIO ROTATIVO SÃO DINÂMICOS,
        // É IMPORTANTE QUE TENHA MALEABILIDADE DE VISUALIZAÇÃO DE DADOS
        // ENTÃO SERÁ CRIADO UMA FRAMEWORK DE RENDERIZAÇÃO DE DESIGN UTILIZANDO SIGNALR (WEBSOCKET)
        // E JAVASCRIPT NO INTERNET EXPLORER 11
        // POSSIVELMENTE PODERÁ SER USADO BOOTSTRAP TAMBÉM
        // https://www.youtube.com/watch?v=pNfSOBzHd8Y
        // https://stackoverflow.com/questions/31251720/ie-11-signalr-not-working -> IE DOESN'T WORKING WITH SIGNALR

        // https://stackoverflow.com/questions/6209161/extract-the-current-dom-and-print-it-as-a-string-with-styles-intact
        // SÓ HÁ UMA MANEIRA DE EXPORTAR ESSA PÁGINA SEM PERDER OS DADOS DO WEBSOCKET
        // EXTRAI O HTML COM AS MODIFICAÇÕES DO DOM, SALVA EM UM ARQUIVO TEMPORÁRIO E FAZ A REQUISIÇÃO DA EXPORTAÇÃO

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
                            ShowProducts();
                            break;
                        case "1":
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

        private void ShowProducts()
        {
            this.GridProducts.Visibility = Visibility.Visible;
            this.GridProductToAdd.Visibility = Visibility.Collapsed;
        }
        private void ShowFinish()
        {
            this.GridProducts.Visibility = Visibility.Collapsed;
            this.GridProductToAdd.Visibility = Visibility.Visible;
        }

        private WebBrowser webBrowser = Helper.MyWebBrowser;

        string previousTextSearch = string.Empty;

        public InventoryTurnover()
        {
            InitializeComponent();
            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Pesquisa", 0, true),
                new MenuItem(Name: "Inventário Rotativo", 1, false)
            };
            CurrentMenuItem = Menu[0];
            this.ButtonPrint.IsEnabled = true;
            this.ButtonExport.IsEnabled = true;
        }

        bool PrintORExport = false;
        string Document = string.Empty;

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Helper.SetOptionsToPrint();
            if (PrintORExport)
            {
                webBrowser.PrintDocument();
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
            webBrowser.LoadCompleted -= WebBrowser_LoadCompleted;
        }

        private string DocumentText(WebBrowser webBrowser)
        {
            HTMLDocument doc = webBrowser.Document as HTMLDocument;

            var collection = doc.getElementsByTagName("html");
            string html = string.Empty;
            foreach (IHTMLElement item in collection)
            {
                html = item.outerHTML;
            }
            return html;
        }

        private void SearchProduct_Click(object sender, RoutedEventArgs e)
        {
            var query = TxtSearchProduct.Text;

            if (previousTextSearch == query)
                return;
            previousTextSearch = query;
            Task.Run(() => Search(query));
        }

        private void ButtonSearchProducts_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentMenuItem = Menu[0];
        }

        private void ButtonInventoryTurnover_Click(object sender, RoutedEventArgs e)
        {
            this.InventoryTurnoverDataGrid.Visibility = Visibility.Visible;
            this.ProductToAddDataGrid.Visibility = Visibility.Collapsed;
        }

        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            string query = TxtSearchProduct.Text;
            Task.Run(() => { Search(query); });
        }
        List<InventoryOfficerPreview.Product> products;
        List<InventoryOfficerPreview.Product> productsAdded = new List<InventoryOfficerPreview.Product>();
        private void Search(string product)
        {
            if (product.Trim() == string.Empty)
                return;
            products = new List<InventoryOfficerPreview.Product>();
            this.ProductToAddDataGrid.Dispatcher.Invoke(new Action (() => { this.ProductToAddDataGrid.ItemsSource = products;  this.ProductToAddDataGrid.Items.Refresh(); }));

            List<ModelsLibraryCore.ConsumptionProduct> listProducts;
            listProducts = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.ServerAPI, $"generalproduct/search/{product}").ToString(), Helper.Authentication);
            foreach (var item in listProducts)
            {
                products.Add(new InventoryOfficerPreview.Product(item.Id, item.Code, item.Description, item.Stock, item.Unity, item));
            }
            
            this.ProductToAddDataGrid.Dispatcher.Invoke(new Action (() => { this.ProductToAddDataGrid.ItemsSource = products;  this.ProductToAddDataGrid.Items.Refresh(); }));
        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //ENVIAR MENSAGEM PARA O CLIENTE...
            var product = ((FrameworkElement)sender).DataContext as InventoryOfficerPreview.Product;
            //var productjson = product.ToJson();
            if (productsAdded.Contains(product))
            {
                productsAdded.Remove(product);
            }
            else
            {
                productsAdded.Add(product);
            }

            this.ProductToAddDataGrid.UnselectAll();
            this.InventoryTurnoverDataGrid.UnselectAll();
            this.InventoryTurnoverDataGrid.ItemsSource = productsAdded;
            this.InventoryTurnoverDataGrid.Items.Refresh();
        }

        private void InventoryTurnoverDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var u = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter && u != null)
            {
                e.Handled = true;
                var datagrid = sender as DataGrid;


                if (SelectedRow(datagrid.Items[datagrid.SelectedIndex]))
                {
                    products.RemoveAt(this.InventoryTurnoverDataGrid.SelectedIndex);
                    this.InventoryTurnoverDataGrid.Items.Refresh();
                }
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                DataGridRow dgr = sender as DataGridRow;
                if (SelectedRow(dgr.Item))
                {
                    products.RemoveAt(this.InventoryTurnoverDataGrid.SelectedIndex);
                    this.InventoryTurnoverDataGrid.Items.Refresh();
                }
            }
        }

        private bool SelectedRow(object item)
        {
            InventoryOfficerPreview.Product stock = item as InventoryOfficerPreview.Product;
            VisualizeProduct visualizeProduct = new VisualizeProduct(stock.InformationProduct);
            if (visualizeProduct.ShowDialog() == true)
            {
                stock.InformationProduct = visualizeProduct.Product;
                return visualizeProduct.RemovedProduct;
            }
            return false;
        }

        private void ProductToAddDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            DataGrid grid = sender as DataGrid;
            var item = grid.GetObjectFromDataGridRow();

            e.Handled = true;
            if (SelectedRow(item))
            {
                products.RemoveAt(this.ProductToAddDataGrid.SelectedIndex);
                this.ProductToAddDataGrid.Items.Refresh();
            }
        }

        private void InventoryTurnoverDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            DataGrid grid = sender as DataGrid;
            var item = grid.GetObjectFromDataGridRow();

            e.Handled = true;
            if (SelectedRow(item))
            {
                products.RemoveAt(this.ProductToAddDataGrid.SelectedIndex);
                this.ProductToAddDataGrid.Items.Refresh();
            }
        }

        private void ProductToAddDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentMenuItem = Menu[1];
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentMenuItem = Menu[0];
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            InventoryOfficerPreview preview = new InventoryOfficerPreview(productsAdded);
            Document = preview.RenderizeHTML();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            webBrowser.NavigateToString(Document);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            InventoryOfficerPreview preview = new InventoryOfficerPreview(productsAdded);
            Document = preview.RenderizeHTML();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            webBrowser.NavigateToString(Document);
        }

        private void InventoryTurnoverDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void TxtSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            var query = TxtSearchProduct.Text;
            if (e.Key == Key.Enter)
            {
                if (previousTextSearch == query)
                    return;

                Task.Run(() => Search(query));
            }
        }
    }
}
