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
            var nameVendor = VendorTextBox.Text;
            var telephoneVendor = TelephoneTextBox.Text;
            new Task(() =>
            {
                ModelsLibraryCore.Vendor vendor = new ModelsLibraryCore.Vendor()
                {
                    Name = nameVendor,
                    Telephone = telephoneVendor
                };
                var result = APIClient.PostData(new Uri(Helper.Server, "Vendor/Add/").ToString(), vendor, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();
        }
    }
}
