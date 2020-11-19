using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebAssemblyLibrary;
using WebAssemblyLibrary.Client;

namespace SCM2020___Client.Frames.Listing
{
    /// <summary>
    /// Interação lógica para ListPermanentProduct.xam
    /// </summary>
    public partial class ListPermanentProduct : UserControl
    {
        class PermanentProduct
        {
            public int SKU { get; }
            public string Description { get; }
            public string Patrimony { get; }
            public string Group { get; }
            public string WorkOrder { get; }

            public PermanentProduct(int SKU, string Description, string Patrimony, string Group, string WorkOrder)
            {
                this.SKU = SKU;
                this.Description = Description;
                this.Patrimony = Patrimony;
                this.Group = Group;
                this.WorkOrder =  (WorkOrder == null) ? string.Empty : WorkOrder;
            }
        }
        private WebBrowser WebBrowser = new WebBrowser();
        private List<PermanentProduct> _ListPermanentProduct = new List<PermanentProduct>();
        public ListPermanentProduct()
        {
            InitializeComponent();
            var permanentProducts = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.ServerAPI, "permanentproduct").ToString(), Helper.Authentication);
            foreach (var permanentProduct in permanentProducts)
            {
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.ServerAPI, $"generalproduct/{permanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                var infoGroup = APIClient.GetData<ModelsLibraryCore.Group>(new Uri(Helper.ServerAPI, $"group/{infoProduct.Group}").ToString(), Helper.Authentication);

                PermanentProduct product = new PermanentProduct(infoProduct.Code, infoProduct.Description, permanentProduct.Patrimony, infoGroup.GroupName, permanentProduct.WorkOrder);
                this._ListPermanentProduct.Add(product);
            }
            this.ListPermanentProductDataGrid.ItemsSource = _ListPermanentProduct;
            this.ListPermanentProductDataGrid.Items.Refresh();
        }

        private void ListPermanentProductDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            Export();
        }

        private void Export()
        {
            Helper.SetOptionsToPrint();
            string printer = Helper.GetPrinter("PDF");
            string tempFile = string.Empty;
            try
            {
                //Declara sinal para sincronismo em diferentes threads
                ManualResetEvent receiveDone = new ManualResetEvent(false);

                //Obter o DOM atual
                string DOM = string.Empty;
                Helper.Client.Receive("ReceiveMessage", (window, message) =>
                {
                    Console.WriteLine("{0}, {1}", window, message);
                    if (window == "SetDOM")
                    {
                        DOM = message;

                        tempFile = Helper.GetTempFilePathWithExtension(".tmp");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(tempFile, false))
                        {
                            file.Write(DOM);
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
                });

                Helper.Client.Send("SendMessage", "GetDOM", "");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro durante exportação", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.PrintDocument();
        }

        private void ListPermanentProductDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
