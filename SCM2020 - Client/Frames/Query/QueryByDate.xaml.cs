using ModelsLibraryCore.RequestingClient;
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
    /// Interação lógica para screen.xam
    /// </summary>
    public partial class QueryByDate : UserControl
    {
        public QueryByDate()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //ModelsLibraryCore.RequestingClient.APIClient.GetData<>()
            /*
             CODIGO -> GENERALPRODUCT
             DESC -> GENERALPRODUCT
             ENTRADA NO ESTOQUE -> AUXILIARPRODUCT INPUT BY VENDOR
             ESTOQUE ATUAL -> GENERALPRODUCT
             SAÍDA -> AUXILIARPRODUCT OUTPUT
             ESTOQUE MÍNIMO -> GENERALPRODUCT
             ESTOQUE MÁXIMO -> GENERALPRODUCT
             */
            int id = 0;
            APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"/generalproduct/{id}").ToString(), Helper.Authentication);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListingDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void ShowByDateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void InitialDate_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void FinalDate_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
