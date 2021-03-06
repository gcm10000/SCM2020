﻿using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interação lógica para QueryUsers.xam
    /// </summary>
    public partial class QueryUsers : UserControl
    {
        bool PrintORExport = false;
        string Document = string.Empty;
        WebBrowser webBrowser = Helper.MyWebBrowser;
        Templates.Query.QueryUsers queryUsers;
        public QueryUsers()
        {
            InitializeComponent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string queryUser = TxtSearch.Text;
                Task.Run(() => Search(queryUser));
            }
        }

        private void SearchConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            string queryUser = TxtSearch.Text;
            Task.Run(() => Search(queryUser));
        }

        private void Search(string queryUser)
        {
            this.DataGridUsers.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridUsers.Items.Clear(); }));
            ButtonEnable(false);
            
            try
            {
                queryUser = System.Uri.EscapeDataString(queryUser);

                var result = APIClient.GetData<List<InfoUser>>(new Uri(Helper.ServerAPI, $"User/search/{queryUser}").ToString(), Helper.Authentication);
                List<Models.QueryUsers> ListUsers = new List<Models.QueryUsers>();
                foreach (var user in result)
                {
                    Models.QueryUsers userToAdd = new Models.QueryUsers()
                    {
                        Name = user.Name,
                        Register = user.Register,
                        Sector = user.Sector.NameSector,
                        ThirdParty = user.ThirdParty,
                    };
                    this.DataGridUsers.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.DataGridUsers.Items.Add(userToAdd); }));
                    ListUsers.Add(userToAdd);
                }
                queryUsers = new Templates.Query.QueryUsers(ListUsers);
                ButtonEnable(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonEnable(bool isEnable)
        {
            this.ButtonExport.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonExport.IsEnabled = isEnable; }));
            this.ButtonPrint.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.ButtonPrint.IsEnabled = isEnable; }));
        }
        private void UsersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = true;
            
            Document = queryUsers.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            PrintORExport = false;

            Document = queryUsers.RenderizeHtml();
            this.webBrowser.LoadCompleted += WebBrowser_LoadCompleted;
            this.webBrowser.NavigateToString(Document);
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

        private void DataGridUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGridUsers_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }
    }
}
