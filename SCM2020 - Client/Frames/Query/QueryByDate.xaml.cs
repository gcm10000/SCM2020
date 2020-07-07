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
        WebBrowser WebBrowser = Helper.MyWebBrowser;
        public QueryByDate()
        {
            InitializeComponent();
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
            var initialDate = InitialDate.SelectedDate.Value;
            var finalDate = FinalDate.SelectedDate.Value;
            var materialInputByVendorInDate = APIClient.GetData<List<ModelsLibraryCore.MaterialInputByVendor>>(new Uri(Helper.Server, $"input/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialOutputInDate = APIClient.GetData<List<ModelsLibraryCore.MaterialOutput>>(new Uri(Helper.Server, $"output/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);
            var materialDevolutionInDate = APIClient.GetData<List<ModelsLibraryCore.MaterialInput>>(new Uri(Helper.Server, $"devolution/date/{initialDate.Day.ToString()}-{initialDate.Month.ToString()}-{initialDate.Year.ToString()}/{finalDate.Day.ToString()}-{finalDate.Month.ToString()}-{finalDate.Year.ToString()}").ToString(), Helper.Authentication);

            foreach (var item in materialInputByVendorInDate)
            {
                //ADD INFO
            }

            foreach (var item in materialOutputInDate)
            {
                //ADD INFO

            }

            foreach (var item in materialDevolutionInDate)
            {
                //ADD INFO

            }

            APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"/generalproduct/{id}").ToString(), Helper.Authentication);
        }

        private void ShowByDateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;

        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
