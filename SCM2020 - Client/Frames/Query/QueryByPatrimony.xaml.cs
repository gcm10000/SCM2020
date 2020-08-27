using Microsoft.VisualBasic;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para QueryByPatrimony.xam
    /// </summary>
    public partial class QueryByPatrimony : UserControl
    {
        public class ProductDataGrid
        {
            public int Code { get; set; }
            public string Description { get; set; }
            public string Patrimony { get; set; }
            public string WorkOrder { get; set; }
            public string Employee { get; set; }

        }
        public QueryByPatrimony()
        {
            InitializeComponent();
        }

        private void QueryDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string patrimony = TxtSearch.Text;
            Task.Run(() => Search(patrimony));
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string patrimony = TxtSearch.Text;
                Task.Run(() => Search(patrimony));
            }
        }
        private void Search(string patrimony)
        {
            if (patrimony == string.Empty)
                return;

            patrimony = System.Uri.EscapeDataString(patrimony);
            var result = APIClient.GetData<List<ModelsLibraryCore.PermanentProduct>>(new Uri(Helper.Server, $"permamentproduct/Search/{patrimony}").ToString(), Helper.Authentication);
            foreach (var item in result)
            {
                //Pega informações referente ao produto
                var infomationProduct = APIClient.GetData<ModelsLibraryCore.ConsumptionProduct>(new Uri(Helper.Server, $"generalproduct/{item.InformationProduct}").ToString(), Helper.Authentication);
                var workOrder = System.Uri.EscapeDataString(item.WorkOrder);
                //Pega informações referente a ordem de serviço
                var informationOS = APIClient.GetData<ModelsLibraryCore.Monitoring>(new Uri(Helper.Server, $"monitoring/workorder/{workOrder}").ToString(), Helper.Authentication);
                //Pega informações referente ao técnico
                var InfoUser = APIClient.GetData<ModelsLibraryCore.InfoUser>(new Uri(Helper.Server, $"user/InfoUser/{informationOS.EmployeeId}").ToString(), Helper.Authentication);

                ProductDataGrid product = new ProductDataGrid()
                {
                    Code = infomationProduct.Code,
                    Description = infomationProduct.Description,
                    Patrimony = item.Patrimony,
                    WorkOrder = item.WorkOrder,
                    Employee = InfoUser.Name
                };
                //Adiciona no datagrid
                this.QueryDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.QueryDataGrid.Items.Add(product); }));
            }
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
