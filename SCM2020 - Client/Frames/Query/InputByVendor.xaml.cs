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

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para InputByVendor.xam
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;

        //NOTA FISCAL v, DATA DA MOVIMENTAÇÃO v, FORNECEDOR, FUNCIONÁRIO v
        public InputByVendor()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string workOrder = TxtSearch.Text;
            if (e.Key == Key.Enter)
                Search(workOrder);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = TxtSearch.Text;
            Search(workOrder);
        }

        private void Search(string workOrder)
        {
            //Zerar todos os dados anteriores...
            this.Export_Button.IsEnabled = false;
            this.Print_Button.IsEnabled = false;




            this.Export_Button.IsEnabled = true;
            this.Print_Button.IsEnabled = true;
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
