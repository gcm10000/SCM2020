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
            var sectorName = SectorTextBox.Text;
            var strNumbersSector = NumberSectorTextBox.Text;
            var strNumbersSectorParsed = strNumbersSector.Split(new string[] { "," }, StringSplitOptions.None);
            var numbersSector = new List<ModelsLibraryCore.NumberSectors>();
            try
            {
                foreach (var strNumber in strNumbersSectorParsed)
                {
                    int number = Convert.ToInt32(strNumber);
                    numbersSector.Add(new ModelsLibraryCore.NumberSectors() { Number = number });
                }
            }
            catch (Exception)
            {
                MessageBox.Show("A sintaxe de entrada aceita somente números separados por vírgula.", "Erro de sintaxe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new Task(() => 
            {
                ModelsLibraryCore.Sector sector = new ModelsLibraryCore.Sector()
                {
                    NameSector = sectorName,
                    NumberSectors = numbersSector
                };
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "Sector/Add/").ToString(), sector, Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            }).Start();
        }
    }
}
