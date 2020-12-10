using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<ModelsLibraryCore.Sector> Sectors;
        List<ModelsLibraryCore.Business> Businesses;
        List<string> SectorsName;
        List<string> BusinessesName;
        ObservableCollection<ModelsLibraryCore.Sector> SectorsComboBox;
        public Sector SSector;
        public Employee()
        {
            Sectors = APIClient.GetData<List<ModelsLibraryCore.Sector>>(new Uri(Helper.ServerAPI, "sector").ToString(), Helper.Authentication);
            Businesses = APIClient.GetData<List<ModelsLibraryCore.Business>>(new Uri(Helper.ServerAPI, "business").ToString(), Helper.Authentication);
            SectorsComboBox = new ObservableCollection<ModelsLibraryCore.Sector>();
            SectorsName = new List<string>();
            foreach (var sector in Sectors)
            {
                //SectorsName.Add($"{sector.NumberSector}: {sector.NameSector}");
                SectorsComboBox.Add(sector);
            }
            //RESOLVER CADASTRO DE FUNCIONÁRIOS, SETORES OPERANDO OK
            InitializeComponent();
            SectorComboBox.ItemsSource = Sectors;
            Console.WriteLine(SectorComboBox.SelectedValue);
            BusinessesName = new List<string>();
            foreach (var business in Businesses)
            {
                BusinessesName.Add(business.Name);
            }
            BusinessComboBox.ItemsSource = BusinessesName;
        }

        private void BtnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(SectorComboBox.SelectedValue);

            ModelsLibraryCore.SignUpUserInfo employee = new ModelsLibraryCore.SignUpUserInfo
            {
                Name = NameTextBox.Text,
                Register = RegisterTextBox.Text,
                //Editar
                //Occupation = OccupationTextBox.Text,
                Business = (BusinessComboBox.SelectedIndex + 1),
                Sector = (SectorComboBox.SelectedIndex + 1),
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
