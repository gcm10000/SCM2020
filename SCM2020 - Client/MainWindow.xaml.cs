﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
            PopupMovement.VerticalOffset = -8;
            PopupRegister.VerticalOffset = -8;
            PopupQueries.VerticalOffset = -8;
            PopupReport.VerticalOffset = -8;

            PopupMovement.HorizontalOffset = 21;
            PopupRegister.HorizontalOffset = 76;
            PopupQueries.HorizontalOffset = 70;
            PopupReport.HorizontalOffset = 68;

            LoginScreen screen = new LoginScreen();
            screen.ShowDialog();


        }

        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var index = listView.SelectedIndex;
            var item = listView.SelectedItem as ListViewItem;
            if (item == null)
                return;
            //ERRO EM SUB LISTVIEW
            for (int i = 0; i < listView.Items.Count; i++)
            {
                var lvItem = listView.Items[i] as ListViewItem;
                lvItem.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                lvItem.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                lvItem.IsSelected = false;
            }
            item.Background = new SolidColorBrush(Color.FromRgb(0xE9, 0xED, 0xFF));
            item.Foreground = new SolidColorBrush(Color.FromRgb(0x4F, 0x68, 0xFF));            
            
            if (index == 1)
            {
                PopupMovement.IsOpen = true;
            }
            else if (index == 2)
            {
                PopupRegister.IsOpen = true;
            }
            else if (index == 3)
            {
                PopupQueries.IsOpen = true;
            }
            else if (index == 4)
            {
                PopupReport.IsOpen = true;
            }
        }
        private void ListViewMoving_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                switch (index)
                {
                    case 0:
                        FrameContent.Source = new Uri("Frames/Movement/InputByVendor.xaml", UriKind.Relative);
                        break;
                    case 1:
                        FrameContent.Source = new Uri("Frames/Movement/Devolution.xaml", UriKind.Relative);
                        break;
                    case 2:
                        FrameContent.Source = new Uri("Frames/Movement/MaterialOutput.xaml", UriKind.Relative);
                        break;
                    case 3:
                        FrameContent.Source = new Uri("Frames/Movement/Closure.xaml", UriKind.Relative);
                        break;
                }
                PopupMovement.IsOpen = false;
            }
        }
        private void ListBoxRegister_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                switch (index)
                {
                    case 0:
                        FrameContent.Source = new Uri("Frames/Register/Employee.xaml", UriKind.Relative);
                        break;
                    case 1:
                        FrameContent.Source = new Uri("Frames/Register/ConsumpterProduct.xaml", UriKind.Relative);
                        break;
                    case 2:
                        FrameContent.Source = new Uri("Frames/Register/Group.xaml", UriKind.Relative);
                        break;
                    case 3:
                        FrameContent.Source = new Uri("Frames/Register/Vendor.xaml", UriKind.Relative);
                        break;
                    case 4:
                        FrameContent.Source = new Uri("Frames/Register/Sector.xaml", UriKind.Relative);
                        break;
                }
                PopupRegister.IsOpen = false;
            }
        }
        private void ListBoxQueries_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                switch (index)
                {
                    case 0:
                        FrameContent.Source = new Uri("Frames/Query/Movement.xaml", UriKind.Relative);
                        break;
                    case 1:
                        FrameContent.Source = new Uri("Frames/Query/InputByVendor.xaml", UriKind.Relative);
                        break;
                    case 2:
                        FrameContent.Source = new Uri("Frames/Query/StockQuery.xaml", UriKind.Relative);
                        break;
                    case 3:
                        FrameContent.Source = new Uri("Frames/Query/QueryByDate.xaml", UriKind.Relative);
                        break;
                }
                PopupQueries.IsOpen = false;
            }
        }
        private void ListBoxReport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                switch (index)
                {
                    case 0:
                        //FrameContent.Source = new Uri("Frames/Movement/InputByVendor.xaml", UriKind.Relative);
                        break;
                    case 1:
                        //FrameContent.Source = new Uri("Frames/Movement/Devolution.xaml", UriKind.Relative);
                        break;
                    case 2:
                        //FrameContent.Source = new Uri("Frames/Movement/MaterialOutput.xaml", UriKind.Relative);
                        break;
                    case 3:
                        //FrameContent.Source = new Uri("Frames/Movement/Closure.xaml", UriKind.Relative);
                        break;
                    case 4:
                        //FrameContent.Source = new Uri("Frames/Movement/Closure.xaml", UriKind.Relative);
                        break;
                    case 5:
                        FrameContent.Source = new Uri("Frames/Listing/InventoryOfficer.xaml", UriKind.Relative);
                        break;
                    case 6:
                        FrameContent.Source = new Uri("Frames/Listing/InventoryTurnover.xaml", UriKind.Relative);
                        break;
                }
                PopupReport.IsOpen = false;
            }
        }
    }
}
