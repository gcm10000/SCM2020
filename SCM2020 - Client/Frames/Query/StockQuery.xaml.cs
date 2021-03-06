﻿using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para StockQuery.xam
    /// </summary>
    public partial class StockQuery : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        List<Models.StockQuery> products = new List<Models.StockQuery>();
        public StockQuery()
        {
            InitializeComponent();
        }

        private void QueryDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        string previousTextSearch = string.Empty;
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = TxtSearch.Text;

            if (previousTextSearch == query)
                return;
            previousTextSearch = query;
            Task.Run(() => SearchStock(query));
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            var query = TxtSearch.Text;
            if (e.Key == Key.Enter)
            {
                if (previousTextSearch == query)
                    return;

                Task.Run(() => SearchStock(query));
            }
        }
        private void SearchStock(string query)
        {
            List<ModelsLibraryCore.ConsumptionProduct> productsGetted = null;
            Clear();
            previousTextSearch = query;

            query = System.Uri.EscapeDataString(query);
            try
            {
                productsGetted = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.ServerAPI, $"generalproduct/search/{query}").ToString(), Helper.Authentication);

            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Produto não encontrado.", "Produto inexistente", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((productsGetted == null) || (productsGetted.Count == 0))
                return;
            foreach (var item in productsGetted)
            {
                //var myitem = new Models.StockQuery(item);
                products.Add(new Models.StockQuery(item));
            }
            this.DataGridProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridProducts.ItemsSource = products; this.DataGridProducts.Items.Refresh(); }));
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));
        }

        private void Clear()
        {
            products.Clear();
            this.DataGridProducts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridProducts.Items.Refresh(); }));
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = false; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = false; }));
        }

        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
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

        private void BtnUpdateMaterial_Click(object sender, RoutedEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as Models.StockQuery;
            var dialog = new SCM2020___Client.Frames.UpdateMaterial(product.ConsumptionProduct);
            if (dialog.ShowDialog() == true)
            {
                //recebe os valores e atualiza lista
                product.Description = dialog.Product.Description;
                product.ConsumptionProduct = dialog.Product;
                this.DataGridProducts.Items.Refresh();
            }

        }

        private void BtnRemoveMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja apagar este material?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            var product = ((FrameworkElement)sender).DataContext as Models.StockQuery;

            var result = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"generalproduct/remove/{product.ConsumptionProduct.Id}").ToString(), Helper.Authentication);
            MessageBox.Show(result.DeserializeJson<string>());
            products.Remove(product);
            this.DataGridProducts.Items.Refresh();
        }

        private void DataGridProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;

            DataGrid grid = sender as DataGrid;
            var item = grid.GetObjectFromDataGridRow();

            e.Handled = true;
            if (SelectedRow(item))
            {
                products.RemoveAt(this.DataGridProducts.SelectedIndex);
                this.DataGridProducts.Items.Refresh();
            }
        }

        private bool SelectedRow(object item)
        {
            Models.StockQuery stock = item as Models.StockQuery;
            if (stock == null)
                return false;
            VisualizeProduct visualizeProduct = new VisualizeProduct(stock.ConsumptionProduct);
            if (visualizeProduct.ShowDialog() == true)
            {
                stock.ConsumptionProduct = visualizeProduct.Product;
                return visualizeProduct.RemovedProduct;
            }
            return false;
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            StockQueryDocument template = new StockQueryDocument(products);
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            StockQueryDocument template = new StockQueryDocument(products);
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void DataGridProducts_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
    }
}
