using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCM2020___Client.Frames.DialogBox
{
    /// <summary>
    /// Lógica interna para AddAndRemove.xaml
    /// </summary>
    public partial class AddAndRemove : Window
    {
        public AddAndRemove(double QuantityAdded)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
            Quantity_Textbox.Text = QuantityAdded.ToString();
            //if (Quantity_Textbox.Text == "0")
            //    Quantity_Textbox.Text = "";
            Quantity_Textbox.SelectionStart = Quantity_Textbox.Text.Length;
        }

        public double QuantityAdded
        {
            get => quantity;
        }

        private double quantity = 0.0d;


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Helper.IsTextAllowed(e.Text));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity_Textbox.Text == string.Empty)
                Quantity_Textbox.Text = "0";
            quantity = double.Parse(Quantity_Textbox.Text);
            this.DialogResult = true;
            this.Close();
        }

        private void Quantity_Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
            e.Handled = (e.Key == Key.Space);
        }
    }
}
