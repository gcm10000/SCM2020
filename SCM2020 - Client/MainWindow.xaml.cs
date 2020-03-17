using System;
using System.Collections.Generic;
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

namespace SCM2020___Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool menuIsOpened = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            menuIsOpened = true;
            BtnOpenMenu.Visibility = Visibility.Collapsed;
            BtnCloseMenu.Visibility = Visibility.Visible;
        }
        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            menuIsOpened = false;
            BtnOpenMenu.Visibility = Visibility.Visible;
            BtnCloseMenu.Visibility = Visibility.Collapsed;
        }
        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //var item = (sender as ListView).SelectedItem;
            var index = (sender as ListView).SelectedIndex;

            if (index == 1)
            {
                popup.HorizontalOffset = (menuIsOpened) ? 20 : -150;
                popup.VerticalOffset = -8;
                popup.IsOpen = true;
            }
        }
    }
}
