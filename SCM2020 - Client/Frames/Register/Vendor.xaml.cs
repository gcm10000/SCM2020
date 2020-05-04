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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Vendor.xam
    /// </summary>
    public partial class Vendor : UserControl
    {
        public Vendor()
        {
            InitializeComponent();
        }

        private void BtnSaveVendor_Click(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                ModelsLibraryCore.Vendor vendor = new ModelsLibraryCore.Vendor()
                {
                    Name = VendorTextBox.Text,
                    Telephone = TelephoneTextBox.Text
                };
                var result = APIClient.PostData(new Uri(Helper.Server, new Uri("Vendor/Add/")).ToString(), vendor, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();
        }
    }
}
