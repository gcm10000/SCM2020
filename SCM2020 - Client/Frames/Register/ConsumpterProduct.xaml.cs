using System;
using System.Collections.Generic;
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
            ModelsLibraryCore.ConsumptionProduct consumptionProduct = new ModelsLibraryCore.ConsumptionProduct()
            {
                Description = "",
                Group = 0,
                Localization = "",
                Stock = 0,
                Unity = "UN",
                NumberLocalization = 0,
                MininumStock = 0
            };
        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
