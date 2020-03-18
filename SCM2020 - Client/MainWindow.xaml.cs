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
            var index = (sender as ListView).SelectedIndex;

            if (index == 1)
            {
                PopupMovement.HorizontalOffset = (menuIsOpened) ? 21 : -149;
                PopupMovement.VerticalOffset = -8;
                PopupMovement.IsOpen = true;
            }
            else if (index == 2)
            {
                PopupRegister.HorizontalOffset = (menuIsOpened) ? 76 : -93;
                PopupRegister.VerticalOffset = -8;
                PopupRegister.IsOpen = true;
            }
            else if (index == 3)
            {
                PopupQueries.HorizontalOffset = (menuIsOpened) ? 70 : -100;
                PopupQueries.VerticalOffset = -8;
                PopupQueries.IsOpen = true;
            }
            else if (index == 4)
            {
                PopupReport.HorizontalOffset = (menuIsOpened) ? 68 : -102;
                PopupReport.VerticalOffset = -8;
                PopupReport.IsOpen = true;
            }
        }
    }
}
