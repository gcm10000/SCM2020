using MaterialDesignThemes.Wpf;
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

namespace SCM2020___Client.Frames
{
    public class Test
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public PackIconKind Icon { get; set; }
        public Test(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
        public Test(string Key, string Value, PackIconKind Icon)
        {
            this.Key = Key;
            this.Value = Value;
            this.Icon = Icon;
        }

    }
    /// <summary>
    /// Interação lógica para Dashboard.xam
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            List<Test> tests = new List<Test>() 
            {
                new Test("DOC/SM/OS", "123456/21", PackIconKind.Invoice),
                new Test("Fornecedor", "Nova Rio", PackIconKind.FlightTakeoff),
                new Test("Data de Movimentação", "03/03/2021", PackIconKind.CalendarToday), //event
            };
            this.ListView.ItemsSource = tests;
            //string html = System.IO.File.ReadAllText("C:\\Users\\Gabriel\\Desktop\\test.html");
            //this.webBrowser.NavigateToString(html);
        }
    }
}
