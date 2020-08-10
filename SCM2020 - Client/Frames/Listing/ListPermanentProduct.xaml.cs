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
            public string IsUsed { get; }

            public PermanentProduct(int SKU, string Description, string Patrimony, string Group, bool IsUsed)
            {
                this.SKU = SKU;
                this.Description = Description;
                this.Patrimony = Patrimony;
                this.Group = Group;
                this.IsUsed = (IsUsed) ? "Sim" : "Não";
            }
        }

        WebAssemblyLibrary.Client.Client client;

        public ListPermanentProduct()
        {
            InitializeComponent();
            Task.Run(() => { client = new WebAssemblyLibrary.Client.Client(); });
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListPermanentProductDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "templates", "listpermanentproduct.html");
            this.webBrowser.Navigate(path);
            
            var permanentProducts = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.Server, "permanentproduct").ToString(), Helper.Authentication);
            webBrowser.LoadCompleted += (sender, args) =>
            {
                foreach (var permanentProduct in permanentProducts)
                {
                    var infoProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{permanentProduct.InformationProduct}").ToString(), Helper.Authentication);
                    var infoGroup = APIClient.GetData<ModelsLibraryCore.Group>(new Uri(Helper.Server, $"group/{infoProduct.Group}").ToString(), Helper.Authentication);
                    
                    PermanentProduct product = new PermanentProduct(infoProduct.Code, infoProduct.Description, permanentProduct.Patrimony, infoGroup.GroupName, permanentProduct.IsUsed);

                    var productjson = product.ToJson();
                    client.Send("SendMessage", "ContentListPermanentProduct", productjson);

                }
            };
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
