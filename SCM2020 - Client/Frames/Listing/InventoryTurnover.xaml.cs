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


        WebAssemblyLibrary.Client.Client client;
        public InventoryTurnover()
        {
            InitializeComponent();
            Task.Run(() => { client = new WebAssemblyLibrary.Client.Client(); });
            this.Print_Button.IsEnabled = true;
            this.Export_Button.IsEnabled = true;
        }

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
                        var document = DocumentText(webBrowser);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = false;
            this.ButtonInventoryTurnover.IsEnabled = true;
            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "templates", "inventoryturnover.html");
            var content = File.ReadAllText(path);
            this.webBrowser.Navigate(path);

        }

        private void ButtonSearchProducts_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = false;
            this.ButtonInventoryTurnover.IsEnabled = true;
            this.webBrowser.Visibility = Visibility.Collapsed;
            this.ProductToAddDataGrid.Visibility = Visibility.Visible;
            this.SearchGrid.Visibility = Visibility.Visible;
        }

        private void ButtonInventoryTurnover_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonSearchProducts.IsEnabled = true;
            this.ButtonInventoryTurnover.IsEnabled = false;
            this.webBrowser.Visibility = Visibility.Visible;
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

        private void Search(string product)
        {
            this.ProductToAddDataGrid.Items.Clear();

            var listProducts = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.ServerAPI, $"generalproduct/search/{product}").ToString(), Helper.Authentication);
            foreach (var item in listProducts)
            {
                this.ProductToAddDataGrid.Items.Add(item);
            }
        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //ENVIAR MENSAGEM PARA O CLIENTE...
            var product = ((FrameworkElement)sender).DataContext as ModelsLibraryCore.ConsumptionProduct;
            var productjson = product.ToJson();
            client.Send("SendMessage", "ContentInventoryTurnover", productjson);

            this.ProductToAddDataGrid.UnselectAll();
        }
    }
}
