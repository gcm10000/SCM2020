using Microsoft.Win32;
using PdfiumViewer;
using SelectPdf;
//using PdfSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PDF_Print
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            int BrowserVer, RegVal;

            // get the installed IE version
            using (WebBrowser Wb = new WebBrowser())
                BrowserVer = Wb.Version.Major;

            // set the appropriate IE version
            if (BrowserVer >= 11)
                RegVal = 11001;
            else if (BrowserVer == 10)
                RegVal = 10001;
            else if (BrowserVer == 9)
                RegVal = 9999;
            else if (BrowserVer == 8)
                RegVal = 8888;
            else
                RegVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);

            webBrowser1.Navigate(@"C:\Users\Gabriel\Desktop\template\pagina2.html");
        }
        private void PrintPDF(string path)
        {
            //https://stackoverflow.com/questions/30048077/how-to-remove-headers-and-footers-programmatically-in-ie-while-printing-instead
            //https://stackoverflow.com/questions/38383787/webbrowser-print-preview-command
            //var path = @"path\file.pdf";
            //using (var document = PdfDocument.Load(path))
            //{
            //    using (var printDocument = document.CreatePrintDocument())
            //    {

            //        PrintDialog printDlg = new PrintDialog();

            //        // preview the assigned document or you can create a different previewButton for it
            //        printDocument.PrinterSettings = printDlg.PrinterSettings;
            //        printDocument.PrintController = new StandardPrintController();

            //        printDlg.Document = printDocument;
            //        printDlg.AllowSelection = true;
            //        printDlg.AllowSomePages = true;
            //        //Call ShowDialog  
            //        if (printDlg.ShowDialog() == DialogResult.OK) printDocument.Print();
            //        //printDocument.Print();
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintPDF(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var pdf = PdfGenerator.GeneratePdf(webBrowser1.DocumentText, PageSize.A4);
            string output = @"C:\Users\Gabriel\Desktop\template\document.pdf";
            //pdf.Save(@"");

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 71;
            converter.Options.MarginRight = 71;
            converter.Options.MarginTop = 71;
            converter.Options.MarginBottom = 14;

            // set converter rendering engine
            converter.Options.RenderingEngine = RenderingEngine.WebKit;

            // create a new pdf document converting an url
            var doc = converter.ConvertUrl(@"C:\Users\Gabriel\Desktop\template\pagina2.html");

            // save pdf document
            doc.Save(output);
            // close pdf document
            doc.Close();

        }


    }
}
