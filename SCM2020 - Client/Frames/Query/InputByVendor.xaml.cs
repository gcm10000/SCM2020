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
using RazorEngine;
using RazorEngine.Templating;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para InputByVendor.xam
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        public InputByVendor()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string template = "Hello @Model.Name, welcome to RazorEngine!";
            var result =
                Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World" });
        }

    }
}
