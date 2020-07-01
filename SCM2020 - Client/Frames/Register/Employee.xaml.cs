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
        List<Sector> Sectors;
        public Employee()
        {
            InitializeComponent();
            Sectors = APIClient.GetData<List<Sector>>(new Uri(Helper.Server, "sector").ToString(), Helper.Authentication);
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                ModelsLibraryCore.SignUpUserInfo employee = new ModelsLibraryCore.SignUpUserInfo
                {
                    Name = NameTextBox.Text,
                    PJERJRegistration = RegisterTextBox.Text,
                    CPFRegistration = CPFTextBox.Text,
                    Occupation = OccupationTextBox.Text,
                    IsPJERJRegistration = (RegisterTextBox.Text.Trim() == string.Empty),
                    IdSector = (SectorComboBox.SelectedIndex + 1),
                    Password = PasswordBoxTextBox.Password
                };
                var result = APIClient.PostData(new Uri(Helper.Server, new Uri("User/NewUser/")).ToString(), employee, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();


        }
    }
}
