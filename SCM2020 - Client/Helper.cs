using System;
using System.IO;
using System.IO.Packaging;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using System.Drawing.Printing;
using System.Linq;
using System.Management;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://192.168.1.30:52991/api/");
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static string TemplatePath = @"C:\Users\Gabriel\source\repos\gcm10000\SCM2020\SCM2020 - Client\Templates\"; //File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "movement.cshtml"));
        //public static string SCMRegistration { get; set; }
        public static string NameIdentifier { get; set; }
        public static string Role { get; set; }
        public static ModelsLibraryCore.Sector CurrentSector { get; set; }
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }

        /// <summary>
        ///  
        ///  Imprimir documento a partir do WebBrowser. <para></para>
        ///  Este método funciona somente após a página estiver totalmente carregada.
        ///  Utilize este método dentro do evento LoadCompleted do WebBrowser.
        ///  <para />Para mais informações: https://stackoverflow.com/questions/28889315/silent-print-html-file-in-c-sharp-using-wpf
        /// </summary>
        /// <param name="webBrowser">Navegador incorporado para realização da impressão do documento.</param>
        /// <returns></returns>
        /// 
        public static bool PrintDocument(this WebBrowser webBrowser)
        {
            MSHTML.IHTMLDocument2 doc = webBrowser.Document as MSHTML.IHTMLDocument2;
            return doc.execCommand("Print", true, null);
        }
        /// <summary>
        ///  
        ///  Exportar documento utilizando impressora virtual a partir do WebBrowser. <para></para>
        ///  Este método funciona somente após a página estiver totalmente carregada.
        ///  Utilize este método dentro do evento LoadCompleted do WebBrowser.
        /// </summary>
        /// <param name="webBrowser">Navegador incorporado para realização da impressão do documento.</param>
        /// <returns></returns>
        public static void ExportDocument(this WebBrowser webBrowser, string NamePrinter)
        {
            string namePrinter = string.Empty;
            try
            {
                namePrinter = GetDefaultPrinterName();
                SetDefaultPrinter(NamePrinter);
            }
            catch (Exception ex)
            {
                throw new Exception("SetDefaultPrinterException1", ex);
            }
            try
            {
                PrintDocument(webBrowser);
            }
            catch (Exception ex)
            {
                throw new Exception("PrintDocumentException", ex);
            }
            try
            {
                SetDefaultPrinter(namePrinter);
            }
            catch (Exception ex)
            {
                throw new Exception("SetDefaultPrinterException2", ex);
            }
        }

        private static string GetDefaultPrinterName()
        {
            var query = new ObjectQuery("SELECT * FROM Win32_Printer");
            var searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                if (((bool?)mo["Default"]) ?? false)
                {
                    return mo["Name"] as string;
                }
            }

            return null;
        }
        private static bool SetDefaultPrinter(string defaultPrinter)
        {
            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
            {
                using (ManagementObjectCollection objectCollection = objectSearcher.Get())
                {
                    foreach (ManagementObject mo in objectCollection)
                    {
                        if (string.Compare(mo["Name"].ToString(), defaultPrinter, true) == 0)
                        {
                            mo.InvokeMethod("SetDefaultPrinter", null, null);
                            return true;
                        }
                    }
                }
            }
            return false;
        }



        public static int GetInternetExplorerVersion()
        {
            // string strKeyPath = @"HKLM\SOFTWARE\Microsoft\Internet Explorer";
            string strKeyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string[] ls = new string[] { "svcVersion", "svcUpdateVersion", "Version", "W2kVersion" };

            int maxVer = 0;
            for (int i = 0; i < ls.Length; ++i)
            {
                object objVal = Microsoft.Win32.Registry.GetValue(strKeyPath, ls[i], "0");
                string strVal = System.Convert.ToString(objVal);
                if (strVal != null)
                {
                    int iPos = strVal.IndexOf('.');
                    if (iPos > 0)
                        strVal = strVal.Substring(0, iPos);

                    int res = 0;
                    if (int.TryParse(strVal, out res))
                        maxVer = Math.Max(maxVer, res);
                } // End if (strVal != null)

            } // Next i

            return maxVer;
        } // End Function GetBrowserVersion 
        public static string GetPrinter(string[] content)
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                //Multiple contains
                foreach (var item in content)
                {
                    if (!content.Any(x => printer.Contains(item)))
                    {
                        continue;
                    }
                    return printer;
                }
            }
            return string.Empty;
        }
    }
}
