using ModelsLibraryCore.RequestingClient;
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
                var result = APIClient.PostData(new Uri(Helper.Server, "generalProduct/Add/").ToString(), consumptionProduct, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            }).Start();
        }
    }
}
