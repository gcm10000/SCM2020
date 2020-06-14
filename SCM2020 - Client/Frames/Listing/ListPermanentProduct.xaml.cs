using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
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

namespace SCM2020___Client.Frames.Listing
{
    /// <summary>
    /// Interação lógica para ListPermanentProduct.xam
    /// </summary>
    public partial class ListPermanentProduct : UserControl
    {
        public ListPermanentProduct()
        {
            InitializeComponent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchPermanentProduct();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchPermanentProduct();

        }
        private void SearchPermanentProduct()
        {

            try
            {
                var query = TxtSearch.Text;
                Task.Run(() => 
                {
                    //
                    var permanentProduct = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.Server, $"permanentproduct/search/{query}").ToString(), Helper.Authentication);
                    //FILL DATA
                }).Start();
                
            }
            catch
            {

            }


        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListingDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void ListPermanentProductDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
    }
}
