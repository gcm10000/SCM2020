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
                foreach (var user in result)
                {
                    user.NameSector = (user.Sector != null) ? user.Sector.NameSector : null;
                    user.NameBusiness = (user.Business != null) ? user.Business.Name : null;
                }
                this.DataGridUsers.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridUsers.ItemsSource = result; }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                if (profile == null)
                    return;
                EditProfile userManager = new EditProfile(profile);
                if (userManager.ShowDialog() == true)
                {
                    string txtsearch = TxtSearch.Text;
                    Task.Run(new Action(() =>
                    {
                        Search(txtsearch);
                    }));
                }
            }
        }

        private void DataGridUsers_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void SearchUserButton_Click(object sender, RoutedEventArgs e)
        {
            string txtsearch = TxtSearch.Text;
            Task.Run(new Action(() =>
            {
                Search(txtsearch);
            }));
        }
    }
}
