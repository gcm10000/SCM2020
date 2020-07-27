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

            this.webBrowser.NavigateToString("");
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
