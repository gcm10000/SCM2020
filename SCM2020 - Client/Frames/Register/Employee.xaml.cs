using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SectorsVM
    {
        public ObservableCollection<ModelsLibraryCore.Sector> Sectors { get; set; }
    }
    /// <summary>
    /// Interação lógica para Employee.xam
    /// </summary>
    public partial class Employee : UserControl
    {
        public Employee()
        {
            List<ModelsLibraryCore.Sector> Sectors = APIClient.GetData<List<ModelsLibraryCore.Sector>>(new Uri(Helper.ServerAPI, "sector").ToString(), Helper.Authentication);
            List<ModelsLibraryCore.Business> Businesses = APIClient.GetData<List<ModelsLibraryCore.Business>>(new Uri(Helper.ServerAPI, "business").ToString(), Helper.Authentication);
            InitializeComponent();
            SectorComboBox.ItemsSource = Sectors;
            BusinessComboBox.ItemsSource = Businesses;
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (SectorComboBox.SelectedIndex != -1)
            {
                MessageBox.Show("Preencha campo de setor.", "Campo vazio", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ModelsLibraryCore.Sector sector = ((ModelsLibraryCore.Sector)SectorComboBox.SelectedItem);

            ModelsLibraryCore.SignUpUserInfo employee = new ModelsLibraryCore.SignUpUserInfo
            {
                Name = NameTextBox.Text,
                Register = RegisterTextBox.Text,
                Business = (BusinessComboBox.SelectedIndex != -1) ? (int?)((ModelsLibraryCore.Business)BusinessComboBox.SelectedItem).Id : null,
                Sector = sector.Id,
                Password = PasswordBoxTextBox.Password
            };
            new Task(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "User/NewUser").ToString(), employee, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();


        }
    }
}
