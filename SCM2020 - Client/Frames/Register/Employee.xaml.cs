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
    /// Interação lógica para Employee.xam
    /// </summary>
    public partial class Employee : UserControl
    {
        List<ModelsLibraryCore.Sector> Sectors;
        List<string> SectorsName;
        public Employee()
        {
            InitializeComponent();
            Sectors = APIClient.GetData<List<ModelsLibraryCore.Sector>>(new Uri(Helper.ServerAPI, "sector").ToString(), Helper.Authentication);
            SectorsName = new List<string>();
            foreach (var sector in Sectors)
            {
                SectorsName.Add(sector.NameSector);
            }
            SectorComboBox.ItemsSource = SectorsName;
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                ModelsLibraryCore.SignUpUserInfo employee = new ModelsLibraryCore.SignUpUserInfo
                {
                    Name = NameTextBox.Text,
                    Register = RegisterTextBox.Text,
                    //Editar
                    //Occupation = OccupationTextBox.Text,
                    //Business = 
                    Sector = (SectorComboBox.SelectedIndex + 1),
                    Password = PasswordBoxTextBox.Password
                };
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, new Uri("User/NewUser/")).ToString(), employee, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();


        }
    }
}
