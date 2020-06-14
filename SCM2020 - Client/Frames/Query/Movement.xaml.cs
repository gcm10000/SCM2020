using System;
using System.Collections.Generic;
using System.IO;
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
using RazorEngine.Templating; // For extension methods.


namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para Movement.xam
    /// </summary>

    //N° MATRICULA - SOLICITANTE - OS - SITUAÇÃO
    //COD - DESC - QTD - UN - PATRIMÔNIO - MOVIMENTAÇÃO - DATA DA MOVIMENTAÇÃO
    public class QueryMovement
    {
        public int RegistrationSolicitationEmployee { get; set; }
        public string SolicitationEmployee { get; set; }
        public string WorkOrder { get; set; }
        public string Situation { get; set; }
        public Product Product { get; set; }
    }
    public class Product
    {
        public int code { get; set; }
        public string description { get; set; }
        public float quantity { get; set; }
        public string unity { get; set; }
        public string patrimony { get; set; }
        public string movement { get; set; }
        public DateTime MoveDate { get; set; }
    }
    public partial class Movement : UserControl
    {
        static string template = File.ReadAllText(System.IO.Path.Combine(Helper.TemplatePath, "movement.html"));

        public Movement()
        {
            InitializeComponent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //If doesn't exists work order, then show error messageBox 
            //MessageBox.Show("Ordem de serviço inexistente.", "Ordem de serviço inexistente", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            //string template = "Hello @Model.Name, welcome to RazorEngine!";
            //Se achou...
            var templateMovement = new QueryMovement()
            {
                RegistrationSolicitationEmployee = 123,
                WorkOrder = "TESTE12345QEA"
            };
            var result = Engine.Razor.RunCompile(template, "templateKey", typeof(QueryMovement), templateMovement);
            this.webBrowser.NavigateToString(result);
            //https://stackoverflow.com/questions/28889315/silent-print-html-file-in-c-sharp-using-wpf
            //IOleServiceProvider x = this.webBrowser.Document as IOleServiceProvider;

            //this.webBrowser.Refresh(true);
        }
    }
}
