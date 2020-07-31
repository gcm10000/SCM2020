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
        class PermanentProduct
        {
            public int SKU { get; }
            public string Description { get; }
            public string Patrimony { get; }
            public string Group { get; }
            public PermanentProduct(int SKU, string Description, string Patrimony, string Group)
            {
                this.SKU = SKU;
                this.Description = Description;
                this.Patrimony = Patrimony;
                this.Group = Group;
            }
        }

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

            }
            catch (Exception ex)
            {

            }


        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListingDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void ListPermanentProductDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var permanentProducts = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.Server, "permanentproduct").ToString(), Helper.Authentication);

            foreach (var permanentProduct in permanentProducts)
            {
                var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/search/{permanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                var infoGroup = APIClient.GetData<ModelsLibraryCore.Group>(new Uri(Helper.Server, $"group/{infoProduct.Group}").ToString(), Helper.Authentication);
                PermanentProduct product = new PermanentProduct(infoProduct.Code, infoProduct.Description, permanentProduct.Patrimony, infoGroup.GroupName);
            }
        }
    }
}
