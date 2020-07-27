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
    /// Interação lógica para InventoryOfficer.xam
    /// </summary>
    public partial class InventoryOfficer : UserControl
    {
        // INVENTÁRIO OFICIAL EXIBE:
        // CÓDIGO
        // DESCRIÇÃO
        // QUANTIDADE
        // DE TODOS OS PRODUTOS

        // UTILIZAR WEBBROWSER POR TER UMA MELHOR PERFORMANCE 
        // NO TRATAMENTO DE VISUALIZAÇÃO DE DADOS
        // DO QUE DATAGRID

        // INVENTÁRIO OFICIAL NÃO HÁ NECESSIDADE DE ALTERAÇÃO DINÂMICA DOS DADOS
        // BASTA EXIBIR TODOS OS DADOS NUMA TABELA E TER A POSSIBILIDADE DE IMPRESSÃO

        public InventoryOfficer()
        {
            InitializeComponent();

        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<InventoryOfficerPreview.Product> products = new List<InventoryOfficerPreview.Product>();
            var productsServer = APIClient.GetData<List<ModelsLibraryCore.ConsumptionProduct>>(new Uri(Helper.Server, "generalproduct/").ToString(), Helper.Authentication);

            foreach (var product in productsServer)
            {
                InventoryOfficerPreview.Product productInventory = new InventoryOfficerPreview.Product(product.Id, product.Code, product.Description, product.Stock);
                products.Add(productInventory);
            }
            InventoryOfficerPreview preview = new InventoryOfficerPreview(products);

            var html = preview.RenderizeHTML();
            this.webBrowser.NavigateToString(html);
        }
    }
}
