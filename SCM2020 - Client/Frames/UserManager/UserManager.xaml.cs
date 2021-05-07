using ModelsLibraryCore;
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
using System.Windows.Threading;

namespace SCM2020___Client.Frames.UserManager
{
    /// <summary>
    /// Interação lógica para UserManager.xam
    /// </summary>
    public partial class UserManager : UserControl
    {
        Templates.Query.QueryUsers queryUsers;
        public UserManager()
        {
            InitializeComponent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string txtsearch = TxtSearch.Text;
                Task.Run(new Action(() => 
                {
                    Search(txtsearch);
                })); 
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string txtsearch = TxtSearch.Text;
            Task.Run(new Action(() =>
            {
                Search(txtsearch);
            }));

        }

        private void Search(string queryUser)
        {
            if (queryUser.Trim() == string.Empty)
            {
                return;
            }

            this.DataGridUsers.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridUsers.ItemsSource = null; }));

            try
            {
                queryUser = System.Uri.EscapeDataString(queryUser);

                var result = APIClient.GetData<List<InfoUser>>(new Uri(Helper.ServerAPI, $"User/search/{queryUser}").ToString(), Helper.Authentication);
                //List<Models.QueryUsers> ListUsers = new List<Models.QueryUsers>();
                //foreach (var user in result)
                //{
                //    Models.QueryUsers userToAdd = new Models.QueryUsers()
                //    {
                //        Name = user.Name,
                //        Register = user.Register,
                //        Sector = user.Sector.NameSector,
                //        ThirdParty = user.ThirdParty,
                //    };
                //    ListUsers.Add(userToAdd);
                //}
                //queryUsers = new Templates.Query.QueryUsers(ListUsers);
                this.DataGridUsers.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridUsers.ItemsSource = result; }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdateMaterial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveMaterial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == null)
                return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataGrid grid = sender as DataGrid;
                var obj = grid.GetObjectFromDataGridRow();
                InfoUser profile = obj as InfoUser;
                //Client.Models.QueryUsers
                e.Handled = true;
                EditProfile userManager = new EditProfile(profile);
                userManager.ShowDialog();
            }
        }

        private void DataGridUsers_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
