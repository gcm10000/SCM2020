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
    /// Interação lógica para Group.xam
    /// </summary>
    public partial class Group : UserControl
    {
        public Group()
        {
            InitializeComponent();
        }

        private void BtnSaveGroup_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SaveGroup()
        {
            new Task(() => 
            {
                ModelsLibraryCore.Group group = new ModelsLibraryCore.Group()
                { GroupName = GroupTextBox.Text };
                //EDITAR
                var result = APIClient.PostData("", group, Helper.Authentication);
                MessageBox.Show(result);
            }).Start();
        }
    }
}
