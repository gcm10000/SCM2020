﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using SCM2020___Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebAssemblyLibrary;

namespace SCM2020___Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //NOTIFICATION WINDOWS 10:
    //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/send-local-toast-desktop?tabs=msix-sparse
    //https://www.youtube.com/watch?v=WhY9ytvZvKE

    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifyIcon;
        public MainWindow()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = System.Drawing.SystemIcons.Exclamation;
            notifyIcon.BalloonTipTitle = "Controle de Materiais";
            notifyIcon.BalloonTipText = "Controle de Materiais";
            notifyIcon.Text = "Controle de Materiais";
            notifyIcon.Visible = true;

            InitializeComponent();
            InitializeMenu();

            WebAssemblyLibrary.Helper.SetLastVersionIE();
            Helper.MyWebBrowser = WebBrowser;

            this.Closed += MainWindow_Closed;

            //PopupMovement.Closed += PopupMovement_Closed;
            //PopupRegister.Closed += PopupRegister_Closed;
            //PopupQueries.Closed += PopupQueries_Closed;
            //PopupReport.Closed += PopupReport_Closed;

            //PopupMovement.VerticalOffset = -8;
            //PopupRegister.VerticalOffset = -8;
            //PopupQueries.VerticalOffset = -8;
            //PopupReport.VerticalOffset = -8;

            //PopupMovement.HorizontalOffset = 21;
            //PopupRegister.HorizontalOffset = 76;
            //PopupQueries.HorizontalOffset = 70;
            //PopupReport.HorizontalOffset = 68;


            //var t = new System.Windows.Forms.TreeView();




            //Open server local

            //Task.Run(() =>
            //{
            //    Helper.WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder().Build();
            //    Helper.WebHost.Run();
            //    //Default url is http://localhost:5000/
            //});


            //Task.Run(() =>
            //{
            //    DateTime startDt = new DateTime(1970, 1, 1);
            //    TimeSpan timeSpan = DateTime.UtcNow - startDt;
            //    long millis = (long)timeSpan.TotalMilliseconds;
            //    var user = new User()
            //    {
            //        DateTimeConnection = DateTime.Now,
            //        Key = millis,
            //        Id = Helper.NameIdentifier
            //    }.ToJson();

            //    var connection = new HubConnectionBuilder()
            //    .WithUrl("http://localhost:52991/notify?user=" + user)
            //    .Build();

            //    //send
            //    //connection.InvokeCoreAsync("notify", new[] {   } });
            //    connection.On("Receive", (object sender, object message) =>
            //    {
            //        //Console.WriteLine($"{message.Sender.Key} to {message.Destination}: {message.Data}{Environment.NewLine}");
            //    });

            //    bool initialize = false;
            //    connection.On("notify", (string stockMessageJson) =>
            //    {
            //        AlertStockMessage stockMessage = stockMessageJson.DeserializeJson<AlertStockMessage>();
            //        notifyIcon1.BalloonTipText = stockMessage.Message;
            //        notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            //        if (!initialize)
            //        {
            //            initialize = true;
            //            notifyIcon1.BalloonTipClicked += (object sender, EventArgs e) =>
            //            {
            //                //abrir janela do estoque exibindo o produto
            //                MessageBox.Show(stockMessage.Code.ToString());
            //            };
            //        }
            //        notifyIcon1.ShowBalloonTip(30000);
            //    });

            //    connection.StartAsync().Wait();
            //});


            //GroupEmployees groupParent = new GroupEmployees() { Name = "Gerencia", };
            //var result = APIClient.PostData(new Uri(Helper.ServerAPI, "Employee/AddGroup"), groupParent, Helper.Authentication);
            //MessageBox.Show(result.Result);

            ////Only users with role SCM and role Administrator have total access
            //if (!((Helper.Role == ModelsLibraryCore.Roles.Administrator) || (Helper.Role == ModelsLibraryCore.Roles.SCM)))
            //{
            //    //Users with access restrited only does can access the Query and Listing menu
            //    this.MovementItem.Visibility = Visibility.Collapsed;
            //    this.RegisterItem.Visibility = Visibility.Collapsed;
            //}

            //ModelsLibraryCore.Employee employee1 = new ModelsLibraryCore.Employee()
            //{
            //    UsersId = "Gabriel"
            //};
            //ModelsLibraryCore.Employee employee2 = new ModelsLibraryCore.Employee()
            //{
            //    UsersId = "Rafael"
            //};
            //ModelsLibraryCore.Employee employee3 = new ModelsLibraryCore.Employee()
            //{
            //    UsersId = "Miguel"
            //};
            //ModelsLibraryCore.Employee employee4 = new ModelsLibraryCore.Employee()
            //{
            //    UsersId = "Lucas"
            //};


            //List<int> ids = new List<int>() { 1 };

            //var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"Employee/FillEmployeeInGroup/4"), ids, Helper.Authentication);


            //Grupo grupo1 = new Grupo() { Name = "Chefe", SuperiorIds = null, SubalternIds = null, Employees = new List<ModelsLibraryCore.Employee>() { employee1 } };
            //Grupo grupo2 = new Grupo() { Name = "Gerente", SuperiorIds = null, SubalternIds = null, Employees = new List<ModelsLibraryCore.Employee>() { employee2, employee3 } };



        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        #region MenuVertical
        private void InitializeMenu()
        {
            var menuMovement = new List<SubItem>();
            menuMovement.Add(new SubItem("Entrada por Fornecedor", new Uri("Frames/Movement/InputByVendor.xaml", UriKind.Relative)));
            menuMovement.Add(new SubItem("Saída", new Uri("Frames/Movement/MaterialOutput.xaml", UriKind.Relative)));
            menuMovement.Add(new SubItem("Devolução", new Uri("Frames/Movement/Devolution.xaml", UriKind.Relative)));
            menuMovement.Add(new SubItem("Fechamento", new Uri("Frames/Movement/Closure.xaml", UriKind.Relative)));
            menuMovement.Add(new SubItem("Reabrir Ordem de Serviço", new Uri("Frames/Movement/Reopen.xaml", UriKind.Relative)));

            var menuRegister = new List<SubItem>();
            menuRegister.Add(new SubItem("Funcionário", new Uri("Frames/Register/Employee.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Produto", new Uri("Frames/Register/ConsumpterProduct.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Produto Permanente", new Uri("Frames/Register/PermanentProduct.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Grupo", new Uri("Frames/Register/Group.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Fornecedor", new Uri("Frames/Register/Vendor.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Setor", new Uri("Frames/Register/Sector.xaml", UriKind.Relative)));
            menuRegister.Add(new SubItem("Empresa", new Uri("Frames/Register/Business.xaml", UriKind.Relative)));

            var menuQueries = new List<SubItem>();
            menuQueries.Add(new SubItem("Movimentação", new Uri("Frames/Query/Movement.xaml", UriKind.Relative)));
            menuQueries.Add(new SubItem("Entrada por Fornecedor", new Uri("Frames/Query/InputByVendor.xaml", UriKind.Relative)));
            menuQueries.Add(new SubItem("Estoque", new Uri("Frames/Query/StockQuery.xaml", UriKind.Relative)));
            menuQueries.Add(new SubItem("Estoque pela Data", new Uri("Frames/Query/QueryByDate.xaml", UriKind.Relative)));
            menuQueries.Add(new SubItem("Patrimônio", new Uri("Frames/Query/QueryByPatrimony.xaml", UriKind.Relative)));
            menuQueries.Add(new SubItem("Ordem de Serviço por Data", new Uri("Frames/Query/QueryWorkOrderByDate.xaml", UriKind.Relative)));

            var menuListingItem = new List<SubItem>();
            menuListingItem.Add(new SubItem("Inventário Oficial", new Uri("Frames/Listing/InventoryOfficer.xaml", UriKind.Relative)));
            menuListingItem.Add(new SubItem("Inventário Rotativo", new Uri("Frames/Listing/InventoryTurnover.xaml", UriKind.Relative)));
            menuListingItem.Add(new SubItem("Listagem de Permanentes", new Uri("Frames/Listing/ListPermanentProduct.xaml", UriKind.Relative)));
            //menuListingItem.Add(new SubItem("Financeiro (em breve)", new Uri("Frames/Listing/Financial.xaml", UriKind.Relative)));


            var item0 = new ItemMenu("Painel de Controle", new UserControl(), MaterialDesignThemes.Wpf.PackIconKind.ViewDashboardOutline, new Uri("Frames/Usercontrol1.xaml", UriKind.Relative));
            var item1 = new ItemMenu("Movimentações", menuMovement, MaterialDesignThemes.Wpf.PackIconKind.ImportExport);
            var item2 = new ItemMenu("Cadastros", menuRegister, MaterialDesignThemes.Wpf.PackIconKind.AddCircleOutline);
            var item3 = new ItemMenu("Consultas", menuQueries, MaterialDesignThemes.Wpf.PackIconKind.Search);
            var item4 = new ItemMenu("Relatórios", menuListingItem, MaterialDesignThemes.Wpf.PackIconKind.BooksVariant);
            //var item5 = new ItemMenu("Gestão de Usuários", new UserControl(), MaterialDesignThemes.Wpf.PackIconKind.People);

            var s1 = new UserControlMenuItem(item0, this.FrameContent);
            s1.FrameChanged += FrameChanged;
            Menu.Children.Add(s1);

            var s2 = new UserControlMenuItem(item1, this.FrameContent);
            s2.FrameChanged += FrameChanged;
            Menu.Children.Add(s2);
            var s3 = new UserControlMenuItem(item2, this.FrameContent);
            s3.FrameChanged += FrameChanged;
            Menu.Children.Add(s3);
            var s4 = new UserControlMenuItem(item3, this.FrameContent);
            s4.FrameChanged += FrameChanged;
            Menu.Children.Add(s4);
            var s5 = new UserControlMenuItem(item4, this.FrameContent);
            s5.FrameChanged += FrameChanged;
            Menu.Children.Add(s5);
            
            s2.listViewMenu.ToString();
        }

        private void FrameChanged(object dataContext, object sender, EventArgs e)
        {
            LoadMenu();
            var subItemClicked = sender as SubItem;
            var context = dataContext as ItemMenu;
            foreach (UserControlMenuItem userControlMenuItem in this.Menu.Children)
            {
                //Items == Subitems
                userControlMenuItem.listViewMenu.Items.ToString();
                
                if (subItemClicked != null)
                {
                    if ((userControlMenuItem.listViewMenu.Items.Count > 0) && (!userControlMenuItem.listViewMenu.Items.Contains(subItemClicked)))
                    {
                        if (userControlMenuItem.listViewMenu.SelectedIndex != -1)
                        {
                            userControlMenuItem.listViewMenu.SelectedIndex = -1;
                        }
                    }
                }
                if (context.Header.Contains("Painel"))
                {
                    if (userControlMenuItem.listViewMenu.SelectedIndex != -1)
                    {
                        userControlMenuItem.listViewMenu.SelectedIndex = -1;
                    }
                }
                
            }
        }

        #endregion
        //#region popupMenu
        //bool PreviousPopupMovement = false;
        //bool PreviousPopupRegister = false;
        //bool PreviousPopupQueries = false;
        //bool PreviousPopupReport = false;

        //private void PopupMovement_Closed(object sender, EventArgs e)
        //{
        //    if (!ClickedInsideMenu())
        //        PreviousPopupMovement = false;
        //}
        //private void PopupRegister_Closed(object sender, EventArgs e)
        //{
        //    if (!ClickedInsideMenu())
        //        PreviousPopupRegister = false;
        //}
        //private void PopupQueries_Closed(object sender, EventArgs e)
        //{
        //    if (!ClickedInsideMenu())
        //        PreviousPopupQueries = false;
        //}
        //private void PopupReport_Closed(object sender, EventArgs e)
        //{

        //    if (!ClickedInsideMenu())
        //        PreviousPopupReport = false;
        //}

        //private bool ClickedInsideMenu()
        //{
        //    var item = VerticalMenu.SelectedItem as ListViewItem;
        //    return item != null;
        //}

        //#endregion
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Application.Current.Shutdown();
        }



        private void SignIn()
        {
            var signIn = APIClient.MakeSignIn(new Uri(Helper.ServerAPI, "user/login/").ToString(),
    Register: "59450",
    Password: "SenhaSecreta#2020");

            Helper.Authentication = signIn.Headers.Authorization;
            Helper.NameIdentifier = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            Helper.CurrentSector = signIn.Sector;
            Helper.Role = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        }


        //private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var listView = sender as ListView;
        //    var index = listView.SelectedIndex;
        //    var item = listView.SelectedItem as ListViewItem;



        //    if (item == null)
        //        return;
        //    for (int i = 0; i < listView.Items.Count; i++)
        //    {
        //        var lvItem = listView.Items[i] as ListViewItem;
        //        lvItem.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
        //        lvItem.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
        //        lvItem.IsSelected = false;
        //    }
        //    item.Background = new SolidColorBrush(Color.FromRgb(0xE9, 0xED, 0xFF));
        //    item.Foreground = new SolidColorBrush(Color.FromRgb(0x4F, 0x68, 0xFF));
        //    switch (index)
        //    {
        //        case 1:
        //            if ((!PreviousPopupMovement) && (!PopupMovement.IsOpen))
        //            {
        //                PopupMovement.IsOpen = true;
        //                PreviousPopupMovement = true;
        //                PreviousPopupRegister = false;
        //                PreviousPopupQueries = false;
        //                PreviousPopupReport = false;
        //                return;
        //            }
        //            break;
        //        case 2:
        //            if ((!PreviousPopupRegister) && (!PopupRegister.IsOpen))
        //            {
        //                PopupRegister.IsOpen = true;
        //                PreviousPopupRegister = true;
        //                PreviousPopupMovement = false;
        //                PreviousPopupQueries = false;
        //                PreviousPopupReport = false;
        //                return;
        //            }
        //            break;
        //        case 3:
        //            if ((!PreviousPopupQueries) && (!PopupQueries.IsOpen))
        //            {
        //                PopupQueries.IsOpen = true;
        //                PreviousPopupQueries = true;
        //                PreviousPopupMovement = false;
        //                PreviousPopupRegister = false;
        //                PreviousPopupReport = false;
        //                return;
        //            }
        //            break;
        //        case 4:
        //            if ((!PreviousPopupReport) && (!PopupReport.IsOpen))
        //            {
        //                PopupReport.IsOpen = true;
        //                PreviousPopupReport = true;
        //                PreviousPopupMovement = false;
        //                PreviousPopupRegister = false;
        //                PreviousPopupQueries = false;
        //                return;
        //            }
        //            break;
        //        //case 5:
        //        //    PopupUser.IsOpen = true;
        //        //    break;
        //        default:
        //            break;
        //    }

        //    PreviousPopupMovement = false;
        //    PreviousPopupRegister = false;
        //    PreviousPopupQueries = false;
        //    PreviousPopupReport = false;
        //}
        //private void ListViewMoving_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var listView = sender as ListBox;
        //    var item = listView.SelectedItem as ListBoxItem;
        //    var index = listView.SelectedIndex;

        //    if (item != null)
        //    {
        //        Uri source = null;
        //        switch (index)
        //        {
        //            case 0:
        //                source = new Uri("Frames/Movement/InputByVendor.xaml", UriKind.Relative);
        //                break;
        //            case 1:
        //                source = new Uri("Frames/Movement/Devolution.xaml", UriKind.Relative);
        //                break;
        //            case 2:
        //                source = new Uri("Frames/Movement/MaterialOutput.xaml", UriKind.Relative);
        //                break;
        //            case 3:
        //                source = new Uri("Frames/Movement/Closure.xaml", UriKind.Relative);
        //                break;
        //            case 4:
        //                source = new Uri("Frames/Movement/Reopen.xaml", UriKind.Relative);
        //                break;
        //        }
        //        PopupMovement.IsOpen = false;
        //        PreviousPopupMovement = false;
        //        PreviousPopupRegister = false;
        //        PreviousPopupQueries = false;
        //        PreviousPopupReport = false;
        //        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        //        {
        //            FrameWindow frame = new FrameWindow(source);
        //            frame.Show();
        //        }
        //        else
        //        {
        //            FrameContent.Source = source;
        //        }
        //        GC.Collect();
        //    }

        //}
        //private void ListBoxRegister_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var listView = sender as ListBox;
        //    var item = listView.SelectedItem as ListBoxItem;
        //    var index = listView.SelectedIndex;
        //    if (item != null)
        //    {
        //        Uri source = null;
        //        switch (index)
        //        {
        //            case 0:
        //                source = new Uri("Frames/Register/Employee.xaml", UriKind.Relative);
        //                break;
        //            case 1:
        //                source = new Uri("Frames/Register/ConsumpterProduct.xaml", UriKind.Relative);
        //                break;
        //            case 2:
        //                source = new Uri("Frames/Register/PermanentProduct.xaml", UriKind.Relative);
        //                break;
        //            case 3:
        //                source = new Uri("Frames/Register/Group.xaml", UriKind.Relative);
        //                break;
        //            case 4:
        //                source = new Uri("Frames/Register/Vendor.xaml", UriKind.Relative);
        //                break;
        //            case 5:
        //                source = new Uri("Frames/Register/Sector.xaml", UriKind.Relative);
        //                break;
        //            case 6:
        //                source = new Uri("Frames/Register/Business.xaml", UriKind.Relative);
        //                break;
        //        }
        //        PopupRegister.IsOpen = false;
        //        PreviousPopupRegister = false;

        //        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        //        {
        //            FrameWindow frame = new FrameWindow(source);
        //            frame.Show();
        //        }
        //        else
        //        {
        //            FrameContent.Source = source;
        //        }
        //        GC.Collect();
        //    }
        //}
        //private void ListBoxQueries_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var listView = sender as ListBox;
        //    var item = listView.SelectedItem as ListBoxItem;
        //    var index = listView.SelectedIndex;
        //    if (item != null)
        //    {
        //        Uri source = null;
        //        switch (index)
        //        {
        //            case 0:
        //                source = new Uri("Frames/Query/Movement.xaml", UriKind.Relative);
        //                break;
        //            case 1:
        //                source = new Uri("Frames/Query/InputByVendor.xaml", UriKind.Relative);
        //                break;
        //            case 2:
        //                source = new Uri("Frames/Query/StockQuery.xaml", UriKind.Relative);
        //                break;
        //            case 3:
        //                source = new Uri("Frames/Query/QueryByDate.xaml", UriKind.Relative);
        //                break;
        //            case 4:
        //                source = new Uri("Frames/Query/QueryByPatrimony.xaml", UriKind.Relative);
        //                break;
        //            case 5:
        //                source = new Uri("Frames/Query/QueryWorkOrderByDate.xaml", UriKind.Relative);
        //                break;
        //            case 6:
        //                source = new Uri("Frames/Query/QueryUsers.xaml", UriKind.Relative);
        //                break;
        //        }
        //        PopupQueries.IsOpen = false;
        //        PreviousPopupQueries = false;
        //        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        //        {
        //            FrameWindow frame = new FrameWindow(source);
        //            frame.Show();
        //        }
        //        else
        //        {
        //            FrameContent.Source = source;
        //        }
        //        GC.Collect();
        //    }
        //}
        //private void ListBoxReport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var listView = sender as ListBox;
        //    var item = listView.SelectedItem as ListBoxItem;
        //    var index = listView.SelectedIndex;
        //    if (item != null)
        //    {
        //        Uri source = null;
        //        switch (index)
        //        {
        //            case 0:
        //                source = new Uri("Frames/Listing/InventoryOfficer.xaml", UriKind.Relative);
        //                break;
        //            case 1:
        //                source = new Uri("Frames/Listing/InventoryTurnover.xaml", UriKind.Relative);
        //                break;
        //            case 2:
        //                source = new Uri("Frames/Listing/ListPermanentProduct.xaml", UriKind.Relative);
        //                break;
        //            case 3:
        //                source = new Uri("Frames/Listing/Financial.xaml", UriKind.Relative);
        //                break;

        //        }
        //        PopupReport.IsOpen = false;
        //        PreviousPopupMovement = false;
        //        PreviousPopupRegister = false;
        //        PreviousPopupQueries = false;
        //        PreviousPopupReport = false;
        //        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        //        {
        //            FrameWindow frame = new FrameWindow(source);
        //            frame.Show();
        //        }
        //        else
        //        {
        //            FrameContent.Source = source;
        //        }
        //        GC.Collect();
        //    }
        //}

        private void UserItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var source = new Uri("Frames/UserManager/UserManager.xaml", UriKind.Relative);
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                FrameWindow frame = new FrameWindow(source);
                frame.Show();
            }
            else
            {
                FrameContent.Source = source;
            }
            GC.Collect();
        }

        private void DashboardItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var source = new Uri("Frames/Dashboard.xaml", UriKind.Relative);
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                FrameWindow frame = new FrameWindow(source);
                frame.Show();
            }
            else
            {
                FrameContent.Source = source;
            }
            GC.Collect();
        }

        private void LoadMenu()
        {
            this.StackPanelMenuHorizontal.Children.Clear();
            this.GridCursor.Visibility = Visibility.Hidden;
            GridCursor.Margin = new Thickness((0), 0, 0, 0);


            var src = FrameContent.Content;
            //MessageBox.Show(FrameContent.Source.ToString());
            if (Helper.HasProperty(src, "Menu") == false)
                return;
            var localMenu = src.GetType().GetProperty("Menu").GetValue(src, null);
            var menuItems = localMenu as List<SCM2020___Client.Frames.MenuItem>;
            SubscribeEvent((Control)src, "MenuItemEventHandler");
            SubscribeEvent((Control)src, "ScreenChanged");
            foreach (var menuItem in menuItems)
            {
                Button buttonMenu = new Button()
                {
                    Uid = menuItem.IdEvent.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 130,
                    Height = 75,
                    Background = null,
                    BorderBrush = null,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    Content = menuItem.Name,
                    IsEnabled = menuItem.IsEnabled
                };

                buttonMenu.Click += (sender, e) =>
                {
                    //MoveGridCursor((Button)e.Source);
                    dynamic userControl = src;
                    userControl.Button_Click(sender, e);
                };
                this.StackPanelMenuHorizontal.Children.Add(buttonMenu);
            }
            if (this.StackPanelMenuHorizontal.Children.Count == 0)
            {
                this.GridCursor.Visibility = Visibility.Hidden;
            }
            else
            {
                this.GridCursor.Visibility = Visibility.Visible;
            }
        }

        private void SubscribeEvent(Control control, string thisNameMethod)
        {
            if (typeof(Control).IsAssignableFrom(control.GetType()))
            {
                try
                {
                    var myself = this;

                    EventInfo eventInfo = control.GetType().GetEvent(thisNameMethod);
                    if (eventInfo == null)
                        return;
                    // Create the delegate on the test class because that's where the
                    // method is. This corresponds with `new EventHandler(test.WriteTrace)`.
                    Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, myself, thisNameMethod);
                    // Assign the eventhandler. This corresponds with `control.Load += ...`.
                    eventInfo.AddEventHandler(control, handler);
                }
                catch (System.NullReferenceException)
                {
                    return;
                }
            }
        }
        public void MenuItemEventHandler(int IdEvent, bool IsEnabled)
        {
            List<UIElement> UICollection = null;
            this.StackPanelMenuHorizontal.Dispatcher.Invoke(new Action(() => { UICollection = this.StackPanelMenuHorizontal.Children.Cast<UIElement>().ToList(); }));

            foreach (Button button in UICollection)
            {
                button.Dispatcher.Invoke(new Action(() =>
                {
                    if (button.Uid == IdEvent.ToString())
                    {
                        button.IsEnabled = IsEnabled;
                    }
                }));
            }
        }

        public void ScreenChanged(object sender, EventArgs e)
        {
            MoveGridCursor(((SCM2020___Client.Frames.MenuItem)sender).IdEvent);

        }
        private void MoveGridCursor(int index)
        {
            GridCursor.Dispatcher.Invoke(new Action(() => { GridCursor.Margin = new Thickness((130 * index), 0, 0, 0); }));
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
