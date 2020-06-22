using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
//using RazorEngine;
//using RazorEngine.Templating; // For extension methods.


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
        //static string template = File.ReadAllText(System.IO.Path.Combine(Helper.TemplatePath, "movement.html"));

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
            //var templateMovement = new QueryMovement()
            //{
            //    RegistrationSolicitationEmployee = 123,
            //    WorkOrder = "TESTE12345QEA"
            //};
            //var result = Engine.Razor.RunCompile(template, "templateKey", typeof(QueryMovement), templateMovement);
            //this.webBrowser.NavigateToString(result);
            //https://stackoverflow.com/questions/28889315/silent-print-html-file-in-c-sharp-using-wpf

            //this.webBrowser.LoadCompleted += (sender, e) => 
            //{
            //    PrintDialog pd = new PrintDialog
            //    {
            //        PrintTicket = new PrintTicket
            //        {
            //            Duplexing = Duplexing.TwoSidedLongEdge,
            //            OutputColor = OutputColor.Monochrome,
            //            PageOrientation = PageOrientation.Portrait,
            //            PageMediaSize = new PageMediaSize(794, 1122),
            //            InputBin = InputBin.AutoSelect
            //        }
            //    };
            //    //pd.PrintTicket.PageMediaSize.Height
            //    //pd.PrintTicket.PageMediaSize.Width
            //    PrintHtmlDocument paginator = new PrintHtmlDocument(webBrowser, 1089, 1122, 794);
            //    pd.ShowDialog();
            //    pd.PrintDocument(paginator, "customDocument");
            //};
            DoPreview("MEU NOVO TESTE");
            //PrintDialog pd = new PrintDialog();

            //PrintDocument();
            //this.webBrowser.Refresh(true);
        }

        private string _previewWindowXaml =
    @"<Window
        xmlns ='http://schemas.microsoft.com/netfx/2007/xaml/presentation'
        xmlns:x ='http://schemas.microsoft.com/winfx/2006/xaml'
        Title ='Print Preview - @@TITLE'
        Height ='200' Width ='300'
        WindowStartupLocation ='CenterOwner'>
                      <DocumentViewer Name='dv1'/>
     </Window>";

        private string flowDocumentXAML = "";

        internal void DoPreview(string title)
        {
            string fileName = "C:\\Users\\Gabriel\\Desktop\\template\\fevereiro.xps";
            //FlowDocumentScrollViewer visual = (FlowDocumentScrollViewer)(_parent.FindName("fdsv1"));
            /*
                Use Flow Document with template to set to Control DocumentViewer.
                Cloud be interessent to use Flow Document in XAML
             */
            try
            {
                // write the XPS document
                //using (XpsDocument doc = new XpsDocument(fileName, FileAccess.ReadWrite))
                //{
                //    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                //    writer.Write(visual);
                //}

                // Read the XPS document into a dynamically generated
                // preview Window 
                using (XpsDocument doc = new XpsDocument(fileName, FileAccess.Read))
                {
                    FixedDocumentSequence fds = doc.GetFixedDocumentSequence();

                    string s = _previewWindowXaml;
                    s = s.Replace("@@TITLE", title.Replace("'", "&apos;"));

                    using (var reader = new System.Xml.XmlTextReader(new StringReader(s)))
                    {
                        Window preview = System.Windows.Markup.XamlReader.Load(reader) as Window;

                        DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(preview, "dv1") as DocumentViewer;
                        dv1.Document = fds as IDocumentPaginatorSource;


                        preview.ShowDialog();
                    }
                }
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                }
            }
        }
        private void PrintDocument()
        {
            
            // NOTE: this works only when the document as been loaded
            MSHTML.IHTMLDocument2 doc = webBrowser.Document as MSHTML.IHTMLDocument2;
            doc.execCommand("PrintPreview", true, null);
        }

    }

}
