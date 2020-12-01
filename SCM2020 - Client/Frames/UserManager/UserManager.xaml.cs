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

namespace SCM2020___Client.Frames.UserManager
{
    /// <summary>
    /// Interação lógica para UserManager.xam
    /// </summary>
    public partial class UserManager : UserControl
    {
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

        private void Search(string query)
        {

        }

        private void QueryDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void QueryDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Cancel = true;
        }

        private void QueryDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnUpdateMaterial_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveMaterial_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
