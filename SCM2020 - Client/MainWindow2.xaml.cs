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
using System.Windows.Shapes;

namespace SCM2020___Client
{
    /// <summary>
    /// Lógica interna para MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();

            var menuRegister = new List<SubItem>();

            menuRegister.Add(new SubItem("Customer"));
            menuRegister.Add(new SubItem("Providers"));
            menuRegister.Add(new SubItem("Employees"));
            menuRegister.Add(new SubItem("Products"));

            var item0 = new ItemMenu("Register", menuRegister, MaterialDesignThemes.Wpf.PackIconKind.Register);

            //Menu.Children.Add(new UserControlMenuItem(item0));
            //Menu.Children.Add(new UserControlMenuItem(item0));
            //Menu.Children.Add(new UserControlMenuItem(item0));
        }
    }
}
