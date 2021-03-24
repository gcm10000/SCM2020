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

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.PrintDocument();
        }

        //IMPRIMIR E EXPORTAR NO NOVO MÉTODO NÃO EFETUADO
        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListPermanentProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;
            DataGrid grid = sender as DataGrid;
            var item = grid.GetObjectFromDataGridRow();
            e.Handled = true;
        }

        private void ListPermanentProductDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
