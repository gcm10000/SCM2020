using Microsoft.VisualBasic;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Channels;
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
    /// Interação lógica para InputByVendor.xam
    /// </summary>
    public partial class InputByVendor : UserControl
    {
        WebBrowser webBrowser = Helper.MyWebBrowser;
        SCM2020___Client.Templates.Query.InputByVendor template;

        #region MenuBar
        public MenuItem CurrentMenuItem
        {
            get => currentMenuItem;
            private set
            {
                if (value != null)
                {
                    currentMenuItem = value;
                    switch (value.IdEvent.ToString())
                    {
                        case "0":
                            ShowSearch();
                            break;
                        case "1":
                            ShowInfo();

                            break;
                        case "2":
                            ShowProducts();
                            break;
                    }
                    ScreenChanged?.Invoke(value, new EventArgs());

                }
            }
        }

        private MenuItem currentMenuItem;
        public List<MenuItem> Menu { get; private set; }
        public delegate void MenuDelegate(object sender, EventArgs e);
        public delegate void MenuItemEnabled(int IdEvent, bool IsEnabled);
        public event MenuDelegate ScreenChanged;
        public event MenuItemEnabled MenuItemEventHandler;

        private void ShowSearch()
        {
            this.ScrollViewerSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerSearch.Visibility = Visibility.Visible; }));
            this.ScrollViewerInfo.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerInfo.Visibility = Visibility.Collapsed; }));
            this.ScrollViewerFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerFinish.Visibility = Visibility.Collapsed; }));
        }
        private void ShowInfo()
        {
            this.ScrollViewerSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerSearch.Visibility = Visibility.Collapsed; }));
            this.ScrollViewerInfo.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerInfo.Visibility = Visibility.Visible; }));
            this.ScrollViewerFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerFinish.Visibility = Visibility.Collapsed; }));
        }
        private void ShowProducts()
        {
            this.ScrollViewerSearch.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerSearch.Visibility = Visibility.Collapsed; }));
            this.ScrollViewerInfo.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerInfo.Visibility = Visibility.Collapsed; }));
            this.ScrollViewerFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerFinish.Visibility = Visibility.Visible; }));
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            CurrentMenuItem = Menu[int.Parse(button.Uid)];
        }
        private void AllowMenuItems()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { Keyboard.ClearFocus(); }));
            MenuItemEventHandler?.Invoke(1, true);
            MenuItemEventHandler?.Invoke(2, true);
            this.CurrentMenuItem = Menu[1];
        }
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentMenuItem = Menu[1];
        }
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentMenuItem = Menu[2];
        }
        #endregion

        public InputByVendor()
        {
            InitializeComponent();

            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Pesquisa", 0, true),
                new MenuItem(Name: "Informações", 1, false),
                new MenuItem(Name: "Produtos", 2, false)
            };

            CurrentMenuItem = Menu[0];
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string invoice = TxtSearch.Text;
            Task.Run(() =>
            {
                if (e.Key == Key.Enter)
                    Search(invoice);
            });
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            string invoice = TxtSearch.Text;
            Task.Run(() => 
            { 
                Search(invoice);
            });
        }

        private void Search(string invoice)
        {
            //Zerar todos os dados anteriores...
            Clear();

            template = new Templates.Query.InputByVendor(invoice);
            if (template.Products == null)
                return;
            foreach (var product in template.Products)
            {
                this.DataGridProductMovement.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridProductMovement.Items.Add(product); }));
            }

            this.InvoiceText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InvoiceText.Text = template.Invoice; }));
            this.VendorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.VendorTextBox.Text = template.Vendor; }));
            this.RegistrationSCMTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegistrationSCMTextBox.Text = template.SCMRegistration; }));
            this.SCMEmployeeTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SCMEmployeeTextBox.Text = template.SCMEmployee; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = this.WorkOrderDateDatePicker.DisplayDate = template.InvoiceDate; }));

            AllowButtons();
            AllowMenuItems();
        }
        private void Clear()
        {
            template = null;
            MenuItemEventHandler?.Invoke(1, false);
            MenuItemEventHandler?.Invoke(2, false);
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = false; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = false; }));
            this.DataGridProductMovement.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridProductMovement.Items.Clear(); }));

            this.InvoiceText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InvoiceText.Text = string.Empty; }));
            this.VendorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.VendorTextBox.Text = string.Empty; }));
            this.RegistrationSCMTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegistrationSCMTextBox.Text = string.Empty; }));
            this.SCMEmployeeTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SCMEmployeeTextBox.Text = string.Empty; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = null; this.WorkOrderDateDatePicker.DisplayDate = DateTime.Today; }));

        }
        private void AllowButtons()
        {
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));
        }
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;
            Document = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            var result = template.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(result);
        }

        private void WebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Helper.SetOptionsToPrint();
            if (PrintORExport)
            {
                webBrowser.PrintDocument();
            }
            else
            {
                string printer = Helper.GetPrinter("PDF");
                string tempFile = string.Empty;
                try
                {

                    tempFile = Helper.GetTempFilePathWithExtension(".tmp");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(tempFile, true))
                    {
                        file.Write(Document);
                        file.Flush();
                    }

                    //"f=" The input file
                    //"p=" The temporary default printer
                    //"d|delete" Delete file when finished
                    var p = new Process();
                    p.StartInfo.FileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Exporter\\document-exporter.exe");
                    //Fazer com que o document-exporter apague o arquivo após a impressão. Ao invés de utilizar finally. Motivo é evitar que o arquivo seja apagado antes do Document-Exporter possa lê-lo.
                    p.StartInfo.Arguments = $"-p=\"{printer}\" -f=\"{tempFile}\" -d";
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro durante exportação", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            webBrowser.LoadCompleted -= WebBrowser_LoadCompleted;
        }

        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void DataGridProductMovement_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            var scrollViewer = dt.GetScrollViewer();
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (e.Delta > 0)
                    scrollViewer.LineLeft();
                else
                    scrollViewer.LineRight();
                e.Handled = true;
            }
        }


    }
}
