using SCM2020___Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interação lógica para UserControlMenuItem.xam
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        public UserControlMenuItem(ItemMenu itemMenu, Frame frameContent)
        {
            InitializeComponent();

            ExpanderMenu.Visibility = (itemMenu.SubItems == null) ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = (itemMenu.SubItems == null) ? Visibility.Visible : Visibility.Collapsed;
            this.DataContext = itemMenu;
            FrameContent = frameContent;
        }

        public Frame FrameContent { get; }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            var subItem = listView.SelectedItem as SubItem;
            FrameContent.Source = subItem.Source;
        }

        private void ListViewItemMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var itemMenu = item.DataContext as ItemMenu;
            FrameContent.Source = itemMenu.Source;
        }
    }
}
