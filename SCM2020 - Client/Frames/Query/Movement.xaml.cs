using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.Templates.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SCM2020___Client.Frames.Query
{
    /// <summary>
    /// Interação lógica para Movement.xam
    /// </summary>

    //MOUSER ENTER
    //MOUSE LEAVE
    //https://stackoverflow.com/questions/12552809/mousehover-and-mouseleave-events-controlling
    public partial class Movement : UserControl
    {
        List<Templates.Query.Movement.Product> ProductsToShow = null;
        WebBrowser webBrowser = Helper.MyWebBrowser;
        SCM2020___Client.Templates.Query.Movement ResultMovement = null;

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
            this.SearchScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SearchScrollViewer.Visibility = Visibility.Visible; }));
            this.InfoScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoScrollViewer.Visibility = Visibility.Collapsed; }));
            this.ScrollViewerFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerFinish.Visibility = Visibility.Collapsed; }));
        }
        private void ShowInfo()
        {
            this.SearchScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SearchScrollViewer.Visibility = Visibility.Collapsed; }));
            this.InfoScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoScrollViewer.Visibility = Visibility.Visible; }));
            this.ScrollViewerFinish.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ScrollViewerFinish.Visibility = Visibility.Collapsed; }));
        }
        private void ShowProducts()
        {
            this.SearchScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SearchScrollViewer.Visibility = Visibility.Collapsed; }));
            this.InfoScrollViewer.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.InfoScrollViewer.Visibility = Visibility.Collapsed; }));
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
        #endregion

        public Movement()
        {
            InitializeComponent();

            Menu = new List<MenuItem>()
            {
                new MenuItem(Name: "Pesquisa", 0, true),
                new MenuItem(Name: "Informações", 1, false),
                new MenuItem(Name: "Produtos", 2, false)
            };
            
            CurrentMenuItem = Menu[0];

            if (!((Helper.WorkOrderByPass == string.Empty) || (Helper.WorkOrderByPass == null)))
            {
                TxtSearch.Text = Helper.WorkOrderByPass;
                Search(Helper.WorkOrderByPass);
                Helper.WorkOrderByPass = string.Empty;
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string workOrder = TxtSearch.Text;
                Task.Run(() =>  Search(workOrder));
            }
        }

        private void Search(string workOrder)
        {
            //Zerar todos os dados anteriores...
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = false; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = false; }));


            this.OSText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSText.Text = string.Empty; }));
            this.RegisterApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicationTextBox.Text = string.Empty; }));
            this.ApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicationTextBox.Text = string.Empty; }));
            this.SectorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SectorTextBox.Text = string.Empty; }));
            this.SituationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SituationTextBox.Text = string.Empty; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = string.Empty; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.Text = string.Empty; }));
            this.ClosureOSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ClosureOSDatePicker.SelectedDate = null; }));
            if (ProductsToShow != null)
                this.ProductsToShow.Clear();
            MenuItemEventHandler?.Invoke(1, false);
            MenuItemEventHandler?.Invoke(2, false);

            try
            {
                ResultMovement = new SCM2020___Client.Templates.Query.Movement(workOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (ResultMovement == null)
            {
                return;
            }
            //textBox1.Focused = false;

            this.OSText.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.OSText.Text = ResultMovement.WorkOrder; }));
            this.RegisterApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.RegisterApplicationTextBox.Text = ResultMovement.RegisterApplication.ToString(); }));
            this.ApplicationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ApplicationTextBox.Text = ResultMovement.Application; }));
            this.SectorTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SectorTextBox.Text = ResultMovement.Sector; }));
            this.SituationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.SituationTextBox.Text = ResultMovement.Situation; }));
            this.ServiceLocalizationTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ServiceLocalizationTextBox.Text = ResultMovement.ServiceLocalization; }));
            this.WorkOrderDateDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.WorkOrderDateDatePicker.SelectedDate = ResultMovement.WorkOrderDate; }));
            this.ClosureOSDatePicker.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ClosureOSDatePicker.SelectedDate = ResultMovement.ClosureWorkOrder; }));

            ProductsToShow = ResultMovement.Products;
            this.ProductMovementDataGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ProductMovementDataGrid.ItemsSource = ProductsToShow; }));

            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = true; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = true; }));

            AllowMenuItems();
        }
        private void ProductMovementDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
        //True to print, False to export.
        bool PrintORExport = false;
        string Document = string.Empty;

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
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
                    p.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "Exporter\\document-exporter.exe");
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

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[2];
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            string workOrder = TxtSearch.Text;
            Task.Run(() => Search(workOrder));
        }


        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;

            Document = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;

            Document = ResultMovement.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
        }

        private void ProductMovementDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            CurrentMenuItem = Menu[1];
        }
    }

}
