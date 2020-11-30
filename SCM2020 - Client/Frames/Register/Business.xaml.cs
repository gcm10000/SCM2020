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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Business.xam
    /// </summary>
    public partial class Business : Page
    {
        public Business()
        {
            InitializeComponent();
        }

        private void BtnSaveBusiness_Click(object sender, RoutedEventArgs e)
        {
            var business = BusinessTextBox.Text;
            new Task(() =>
            {
                ModelsLibraryCore.Business sector = new ModelsLibraryCore.Business()
                {
                    Name = business
                };
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "Business/Add/").ToString(), sector, Helper.Authentication);
                MessageBox.Show(result.DeserializeJson(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            }).Start();
        }
    }
}
