using SCM2020___Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        public delegate void FrameChangedHandler(object dataContext, object sender, EventArgs e);
        public event FrameChangedHandler FrameChanged;
        public ListView ListView { get => listViewMenu; }
        private SubItem PreviousSelectedItem;
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

            var isShiftDown = (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));

            FrameContent.ContentRendered += FrameContent_ContentRendered;
            
            if (subItem != null)
            {
                if (isShiftDown)
                {
                    FrameWindow frame = new FrameWindow(subItem.Source);
                    frame.Show();
                    listView.SelectionChanged -= ListViewMenu_SelectionChanged;
                    listView.SelectedItem = PreviousSelectedItem;
                    listView.SelectionChanged += ListViewMenu_SelectionChanged;
                }
                else
                {
                    FrameContent.Source = subItem.Source;
                }
            }

            if (PreviousSelectedItem != subItem)
            {
                PreviousSelectedItem = subItem;
            }
        }

        private void ListViewItemMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var itemMenu = item.DataContext as ItemMenu;
            FrameContent.ContentRendered += FrameContent_ContentRendered;
            FrameContent.Source = itemMenu.Source;
            
            //if (this.listViewMenu.SelectedIndex != -1)
            //{
            //    this.listViewMenu.SelectedIndex = -1;
            //}
        }

        private void FrameContent_ContentRendered(object sender, EventArgs e)
        {
            if (FrameChanged != null)
                FrameChanged.Invoke(this.DataContext, this.listViewMenu.SelectedItem, e);

            
            FrameContent.ContentRendered -= FrameContent_ContentRendered;
        }
    }
}
