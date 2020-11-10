using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para InventoryOfficer.xam
    /// </summary>
    public partial class InventoryOfficer : UserControl
    {
        // INVENTÁRIO OFICIAL EXIBE:
        // CÓDIGO
        // DESCRIÇÃO
        // QUANTIDADE
        // DE TODOS OS PRODUTOS

        // UTILIZAR WEBBROWSER POR TER UMA MELHOR PERFORMANCE 
        // NO TRATAMENTO DE VISUALIZAÇÃO DE DADOS
        // DO QUE DATAGRID

        // INVENTÁRIO OFICIAL NÃO HÁ NECESSIDADE DE ALTERAÇÃO DINÂMICA DOS DADOS
        // BASTA EXIBIR TODOS OS DADOS NUMA TABELA E TER A POSSIBILIDADE DE IMPRESSÃO

        public InventoryOfficer()
        {
            InitializeComponent();

        }
        
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            PrintExport();
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            PrintExport();
        }
        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.Export_Button.IsEnabled = true;
            this.Print_Button.IsEnabled = true;
        }

        private void PrintExport()
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<InventoryOfficerPreview.Product> products = new List<InventoryOfficerPreview.Product>();
            var productsServer = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.ServerAPI, "generalproduct/").ToString(), Helper.Authentication);

            foreach (var product in productsServer)
            {
                InventoryOfficerPreview.Product productInventory = new InventoryOfficerPreview.Product(product.Id, product.Code, product.Description, product.Stock, product.Unity);
                products.Add(productInventory);
            }
            InventoryOfficerPreview preview = new InventoryOfficerPreview(products);

            var html = preview.RenderizeHTML();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(html);
            Document = html;

        }
    }
}
