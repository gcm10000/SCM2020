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
    /// Interação lógica para StockQuery.xam
    /// </summary>
    public partial class StockQuery : UserControl
    {
        public StockQuery()
        {
            InitializeComponent();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductToAddDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void QueryDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        string previousTextSearch = string.Empty;
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = TxtSearch.Text;

            if (previousTextSearch == query)
                return;
            previousTextSearch = query;
            SearchStock(query);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            var query = TxtSearch.Text;
            if (e.Key == Key.Enter)
            {
                if (previousTextSearch == query)
                    return;
                previousTextSearch = query;
                SearchStock(query);
            }
        }
        private void SearchStock(string query)
        {
            List<ModelsLibraryCore.ConsumptionProduct> result = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.Server, $"generalproduct/search/{query}").ToString(), Helper.Authentication);
            foreach (var item in result)
            {
                this.QueryDataGrid.Items.Add(item);
            }
        }
    }
}
