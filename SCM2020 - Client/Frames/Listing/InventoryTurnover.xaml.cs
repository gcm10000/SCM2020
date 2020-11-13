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


        public InventoryTurnover()
        {
            InitializeComponent();
            this.Print_Button.IsEnabled = true;
            this.Export_Button.IsEnabled = true;
        }

        private WebBrowser WebBrowser = new WebBrowser();

        bool PrintORExport = false;

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            // faz requisição da string html por websocket
            // coloca na string Document
            //this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            ExportPrint();
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            //this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            ExportPrint();
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            
        }

        private void ExportPrint()
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
                        var document = DocumentText(WebBrowser);
                        document = document.Replace("css/bootstrap.min.css", new System.Uri(System.IO.Path.Combine(Helper.CurrentDirectory, "templates", "css", "bootstrap.min.css")).AbsoluteUri);
                        file.Write(document);
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
            WebBrowser.LoadCompleted -= WebBrowser_LoadCompleted;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = false;
            this.ButtonInventoryTurnover.IsEnabled = true;
            //var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "templates", "inventoryturnover.html");
            //var content = File.ReadAllText(path);
            //this.webBrowser.Navigate(path);

        }

        private void ButtonSearchProducts_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = false;
            this.ButtonInventoryTurnover.IsEnabled = true;
            //this.webBrowser.Visibility = Visibility.Collapsed;
            this.InventoryTurnoverDataGrid.Visibility = Visibility.Collapsed;
            this.InventoryTurnoverDataGrid.Visibility = Visibility.Collapsed;
            this.ProductToAddDataGrid.Visibility = Visibility.Visible;
            this.SearchGrid.Visibility = Visibility.Visible;
        }

        private void ButtonInventoryTurnover_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = true;
            this.ButtonInventoryTurnover.IsEnabled = false;
            //this.webBrowser.Visibility = Visibility.Visible;
            this.InventoryTurnoverDataGrid.Visibility = Visibility.Visible;
            this.ProductToAddDataGrid.Visibility = Visibility.Collapsed;
            this.SearchGrid.Visibility = Visibility.Collapsed;
        }

        private void TxtSearchConsumpterProduct_KeyDown(object sender, KeyEventArgs e)
        {
            string workOrder = TxtSearchConsumpterProduct.Text;
            if (e.Key == Key.Enter)
                Search(workOrder);

        }

        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            Search(TxtSearchConsumpterProduct.Text);
        }
        List<InventoryOfficerPreview.Product> products = new List<InventoryOfficerPreview.Product>();
        List<InventoryOfficerPreview.Product> productsAdded = new List<InventoryOfficerPreview.Product>();
        private void Search(string product)
        {
            this.ProductToAddDataGrid.Items.Clear();
            this.products.Clear();

            List<ModelsLibraryCore.ConsumptionProduct> listProducts;
            listProducts = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.ServerAPI, $"generalproduct/search/{product}").ToString(), Helper.Authentication);
            foreach (var item in listProducts)
            {
                products.Add(new InventoryOfficerPreview.Product(item.Id, item.Code, item.Description, item.Stock, item.Unity, item));
            }
            this.InventoryTurnoverDataGrid.ItemsSource = products;
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
                products.Remove(product);
            }
            else
            {
                productsAdded.Add(product);
            }
            this.ProductToAddDataGrid.UnselectAll();
            this.InventoryTurnoverDataGrid.Items.Refresh();
        }

        private void InventoryTurnoverDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
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
    }
}
