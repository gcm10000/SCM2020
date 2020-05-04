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
    /// Interação lógica para Sector.xam
    /// </summary>
    public partial class Sector : UserControl
    {
        public Sector()
        {
            InitializeComponent();
        }

        private void BtnSaveSector_Click(object sender, RoutedEventArgs e)
        {
            new Task(() => 
            {
                ModelsLibraryCore.Sector sector = new ModelsLibraryCore.Sector()
                {
                    NameSector = SectorTextBox.Text,
                    NumberSector = int.Parse(NumberSectorTextBox.Text)
                };
                var result = APIClient.PostData(new Uri(Helper.Server, new Uri("Sector/Add/")).ToString(), sector, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();
        }
    }
}
