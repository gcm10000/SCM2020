using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace SCM2020___Client.Frames
{
    public class MenuItem
    {
        public MenuItem(string Name, int IdEvent, bool IsEnabled)
        {
            this.Name = Name;
            this.IdEvent = IdEvent;
            this.IsEnabled = IsEnabled;
        }
        public string Name { get; set; }
        public int IdEvent { get; set; }
        public bool IsEnabled { get; set; }
    }
    /// <summary>
    /// Interação lógica para UserControl1.xam
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public MenuItem CurrentMenuItem 
        { 
            get => currentMenuItem;
            private set 
            { 
                if (value != null)
                {
                    currentMenuItem = value;
                    switch (value.IdEvent.ToString())
                    {
                        case "0":
                            ShowInfo();
                            break;
                        case "1":
                            ShowProducts();
                            break;
                        case "2":
                            ShowFinish();
                            break;
                    }
                    ScreenChanged?.Invoke(value, new EventArgs());

                }
            }
        }
        private MenuItem currentMenuItem;
        public List<MenuItem> Menu { get; private set; }
        public delegate void MenuDelegate(object sender, EventArgs e);
        public event MenuDelegate ScreenChanged;

        public UserControl1()
        {
            InitializeComponent();
            Menu = new List<MenuItem>() 
            { 
                new MenuItem(Name: "Informações", 0, true),
                new MenuItem(Name: "Produtos", 1, true),
                new MenuItem(Name: "Finalização", 2, true)
            };
            CurrentMenuItem = Menu[0];

            List<SummaryInfo> infos = new List<SummaryInfo>()
            {
                new SummaryInfo("DOC/SM/OS", "123456/21", PackIconKind.Invoice),
                new SummaryInfo("Fornecedor", "Nova Rio123", PackIconKind.FlightTakeoff),
                new SummaryInfo("Data de Movimentação", "03/03/2021", PackIconKind.CalendarToday), //event
            };
            this.ListView.ItemsSource = infos;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            CurrentMenuItem = Menu[int.Parse(button.Uid)];
        }
        
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
        }

        private void ShowInfo()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Visible;
            this.GridProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;

        }
        private void ShowProducts()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridProducts.Visibility = Visibility.Visible;
            this.ScrollViewerFinish.Visibility = Visibility.Collapsed;
        }
        private void ShowFinish()
        {
            this.ScrollViewerInfo.Visibility = Visibility.Collapsed;
            this.GridProducts.Visibility = Visibility.Collapsed;
            this.ScrollViewerFinish.Visibility = Visibility.Visible;
        }

        
        private void ButtonNext1_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }

        private void ButtonNext2_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];

        }

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
