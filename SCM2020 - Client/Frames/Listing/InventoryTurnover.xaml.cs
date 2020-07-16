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
    /// Interação lógica para InventoryTurnover.xam
    /// </summary>
    public partial class InventoryTurnover : UserControl
    {
        // INVENTÁRIO ROTATIVO EXIBE:
        // CÓDIGO
        // DESCRIÇÃO
        // QUANTIDADE
        // DE PRODUTOS SELECIONADOS
        // FILTRAR PRODUTO POR CÓDIGO OU DESCRIÇÃO
        // MULTIPLE CONTAINS POR PALAVRAS .Split(' ')

        // UTILIZAR WEBBROWSER POR TER UMA MELHOR PERFORMANCE 
        // NO TRATAMENTO DE VISUALIZAÇÃO DE DADOS
        // DO QUE DATAGRID

        // COMO QUE OS DADOS DO INVENTÁRIO ROTATIVO SÃO DINÂMICOS,
        // É IMPORTANTE QUE TENHA MALEABILIDADE DE VISUALIZAÇÃO DE DADOS
        // ENTÃO SERÁ CRIADO UMA FRAMEWORK DE RENDERIZAÇÃO DE DESIGN UTILIZANDO SIGNALR (WEBSOCKET)
        // E JAVASCRIPT NO INTERNET EXPLORER 11
        // POSSIVELMENTE PODERÁ SER USADO BOOTSTRAP TAMBÉM
        // https://www.youtube.com/watch?v=pNfSOBzHd8Y
        // https://stackoverflow.com/questions/31251720/ie-11-signalr-not-working -> IE DOESN'T WORKING WITH SIGNALR
        public InventoryTurnover()
        {
            InitializeComponent();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
