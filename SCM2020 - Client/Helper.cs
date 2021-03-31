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
using System.Drawing;
using Microsoft.Win32;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SCM2020___Client
{
    static partial class Helper
    {
        public static readonly string CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        /// <summary>
        /// Endereço do servidor.
        /// </summary>
        public static Uri Server = new Uri("http://localhost:52991/");
        /// <summary>
        /// Endereço do controle de API.
        /// </summary>
        public static Uri ServerAPI = new Uri(Server, "api/");
        /// <summary>
        /// Endereço do controle de notificações.
        /// </summary>
        public static Uri ServerNotify = new Uri(Server, "notify");
        public static Uri AppendQuery(this Uri uri, string key, string value)
        {
            string queryToAppend = $"{key}={value}";
            UriBuilder baseUri = new UriBuilder(uri);

            if (baseUri.Query != null && baseUri.Query.Length > 1)
                baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
            else
                baseUri.Query = queryToAppend;

            return baseUri.Uri;
        }
        /// <summary>
        /// Autenticação das credenciais.
        /// </summary>
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        /// <summary>
        /// Regex com disposição única a números.
        /// </summary>
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        /// <summary>
        /// Id do usuário logado.
        /// </summary>
        public static string NameIdentifier { get; set; }
        public static string Role { get; set; }
        public static ModelsLibraryCore.Sector CurrentSector { get; set; }
        public static WebBrowser MyWebBrowser { get; set; }
        public static WebAssemblyLibrary.Client.Client Client { get; set; }
        public static IWebHost WebHost { get; internal set; }
        public static string WorkOrderByPass { get; set; }
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
        public static string RelativeTime(this DateTime pastDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - pastDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "agora" :  "há " + ts.Seconds + " segundos";

            if (delta < 2 * MINUTE)
                return "há 1 minuto";

            if (delta < 45 * MINUTE)
                return "há " + ts.Minutes + " minutos";

            if (delta < 90 * MINUTE)
                return "há 1 hora";

            if (delta < 24 * HOUR)
                return "há " + ts.Hours + " horas";

            if (delta < 48 * HOUR)
                return "há 1 dia";

            if (delta < 30 * DAY)
                return "há " + ts.Days + " dias";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            var directory = Path.Combine(path, "SCM");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return Path.Combine(directory, fileName);
        }
        public static BitmapSource LoadImage(this byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                var decoder = BitmapDecoder.Create(ms,
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }
        public static T GetChildOfType<T>(this DependencyObject depObj)
    where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
        public static void SetOptionsToPrint()
        {
            string strKey = "Software\\Microsoft\\Internet Explorer\\PageSetup";
            bool bolWritable = true;
            RegistryKey oKey = Registry.CurrentUser.OpenSubKey(strKey, bolWritable);
            oKey.SetValue("font", "");
            oKey.SetValue("header", "");
            oKey.SetValue("footer", "");
            oKey.SetValue("margin_bottom", "0.75000"); //Margem é dada em polegadas
            oKey.SetValue("margin_left", "0.75000");
            oKey.SetValue("margin_right", "0.75000");
            oKey.SetValue("margin_bottom", "0.75000");
            oKey.SetValue("margin_top", "0.75");
            oKey.SetValue("Print_Background", "yes");
            oKey.SetValue("Shrink_To_Fit", "yes");
        }

        public static ScrollViewer GetScrollViewer(this UIElement element)
        {
            if (element == null) return null;

            ScrollViewer retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                {
                    retour = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
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
        public static string GetPrinter(params string[] content)
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
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static object GetObjectFromDataGridRow(this DataGrid grid)
        {
            try
            {
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    //This is the code which helps to show the data when the row is double clicked.
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    return dgr.Item;
                }
                return new ArgumentNullException("DataGrid encontra-se nulo ou não há um item selecionado.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
