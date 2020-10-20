using ModelsLibraryCore.RequestingClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Product.xam
    /// </summary>
    public partial class ConsumpterProduct : UserControl
    {
        public ConsumpterProduct()
        {
            InitializeComponent();
            Uri groupUri = new Uri(Helper.Server, "group/");
            var groups = APIClient.GetData<List<ModelsLibraryCore.Group>>(groupUri.ToString());
            var nameGroups = groups.Select(x => x.GroupName).ToList();
            this.GroupComboBox.ItemsSource = nameGroups;
            RequestCodeAvaliable();
        }
        private void RequestCodeAvaliable()
        {
            Uri nextNumberUri = new Uri(Helper.Server, "generalproduct/nextnumber");
            var number = APIClient.GetData<int>(nextNumberUri.ToString());

            this.CodeTextBox.Text = number.ToString();
        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            ModelsLibraryCore.ConsumptionProduct consumptionProduct = new ModelsLibraryCore.ConsumptionProduct()
            {
                Code = int.Parse(CodeTextBox.Text),
                Description = DescriptionTextBox.Text,
                Group = (GroupComboBox.SelectedIndex + 1),
                Localization = LocalizationTextBox.Text,
                MininumStock = double.Parse(MininumStockTextBox.Text),
                MaximumStock = double.Parse(MaximumStockTextBox.Text),
                NumberLocalization = uint.Parse(NumberLocalizationTextBox.Text),
                Stock = double.Parse(StockTextBox.Text),
                Unity = UnityTextBox.Text,
                Photo = null
            };
            new Task(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.Server, "generalProduct/Add/"), consumptionProduct, Helper.Authentication);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(result.Result), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (result.Result.Contains("utilizado"))
                    {
                        System.Windows.Forms.DialogResult messageBoxResult = (System.Windows.Forms.DialogResult)MessageBox.Show("O SKU se encontra utilizado. Gostaria cadastrar este produto utilizando outro SKU?", "Pergunta", MessageBoxButton.YesNo);
                        if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            RequestCodeAvaliable();
                            MessageBox.Show("SKU alterado. Por favor, tente novamente o cadastro do produto.");
                        }
                    }
                }
            }).Start();
        }
    }
}
