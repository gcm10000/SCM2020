using Microsoft.AspNetCore.Hosting;
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
using SCM2020___Client.Models;
using System.Threading;
using System.IO;

namespace SCM2020___Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    //popup width x height = 600 x 160
    //NOTIFICATION WINDOWS 10:
    //https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/send-local-toast-desktop?tabs=msix-sparse
    //https://www.youtube.com/watch?v=WhY9ytvZvKE
    //SOM DE NOTIFICAÇÃO DA ALEXA: https://www.youtube.com/watch?v=SlVluRNN8aw
    //REPRODUZIR SOM WAV EM BYTES: https://stackoverflow.com/questions/21976011/playing-byte-in-c-sharp


    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifyIcon;
        List<Notification> notifications;

        public MainWindow()
        {
            InitializeComponent();
            ChooseAccess(Helper.Role); //Recomendável que este método esteja na inicialização do menu
            InitializeMenu();
            InitializeNotifyIcon();
            InitializeNotification();
            

            WebAssemblyLibrary.Helper.SetLastVersionIE();
            Helper.MyWebBrowser = WebBrowser;

            this.Closed += MainWindow_Closed;


            //Open server local

            //Task.Run(() =>
            //{
            //    Helper.WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder().Build();
            //    Helper.WebHost.Run();
            //    //Default url is http://localhost:5000/
            //});

            Task.Run(() =>
            {
                DateTime startDt = new DateTime(1970, 1, 1);
                TimeSpan timeSpan = DateTime.UtcNow - startDt;
                long millis = (long)timeSpan.TotalMilliseconds;
                var user = new User()
                {
                    DateTimeConnection = DateTime.Now,
                    Key = millis,
                    Id = Helper.NameIdentifier
                }.ToJson();

                var connection = new HubConnectionBuilder()
                    .WithUrl(Helper.ServerNotify.AppendQuery("user", user))
                    .Build();

                //send
                //connection.InvokeCoreAsync("notify", new[] { } );
                connection.On("Receive", (object sender, object message) =>
                {
                    Message msg = message as Message;

                    Console.WriteLine($"{msg.Sender.Key} to {msg.Destination}: {msg.Data}{Environment.NewLine}");
                });

                bool initialize = false;
                connection.On("notify", (string stockMessageJson) =>
                {
                    //Salvar notificações dentro de um arquivo JSON.
                    //Em configurações -> notificações ter a opção de limpar as notificações, e em seguida apagar todo arquivo JSON
                    //Como também limpar toda a lista de notificações.
                    //Exibir no máximo 20 notificações.
                    AlertStockMessage stockMessage = stockMessageJson.DeserializeJson<AlertStockMessage>();
                    notifyIcon.BalloonTipText = stockMessage.Message;
                    notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
                    notifications.Insert(0, new Notification(stockMessage.Message, System.Drawing.SystemIcons.Exclamation, DateTime.UtcNow, stockMessage.Code));
                    //Serializar em JSON a lista notifications
                    //Criptografar em AES
                    //Salvar em arquivo JSON criptografado

                    //ou salvar em servidor?
                    Helper.PlayNotificationSound();


                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        var newestNotificationsCount = notifications.Where(x => x.IsNewest).Count();
                        this.TextBlockNotificationQuantity.Text = (newestNotificationsCount > 9) ? "9+" : newestNotificationsCount.ToString();
                        if (newestNotificationsCount == 0)
                            this.BorderCountNewestNotification.Visibility = Visibility.Hidden;
                        else
                            this.BorderCountNewestNotification.Visibility = Visibility.Visible;

                        this.ListViewNotification.Items.Refresh();
                    });

                    
                    if (!initialize)
                    {
                        initialize = true;
                        notifyIcon.BalloonTipClicked += (object sender, EventArgs e) =>
                        {
                            //abrir janela do estoque exibindo o produto
                            MessageBox.Show(stockMessage.Code.ToString());
                            
                        };
                    }
                    notifyIcon.ShowBalloonTip(30000);
                });

                connection.StartAsync().Wait();
            });


        }

        private void InitializeNotification()
        {
            this.notifications = new List<Notification>();
            this.ListViewNotification.ItemsSource = notifications;
            this.TextBlockNotificationQuantity.Text = (notifications.Where(x => x.IsNewest).Count() > 9) ? "9+" : notifications.Count.ToString();
            if (notifications.Count == 0)
                this.BorderCountNewestNotification.Visibility = Visibility.Hidden;
            else
                this.BorderCountNewestNotification.Visibility = Visibility.Visible;
        }

        private void PopupBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.PopoutNotification.StaysOpen = true;
                if (!this.ListViewNotification.IsVisible)
                {
                    foreach (var notification in notifications)
                    {
                        notification.RelativeTime = Helper.RelativeTime(notification.DateTime);
                        notification.IsNewest = false;
                        this.ListViewNotification.Items.Refresh();
                    }
                    this.BorderCountNewestNotification.Visibility = Visibility.Hidden;
                }
            }
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel stackPanelItem = sender as StackPanel;
            stackPanelItem.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E6E6E6"));
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel stackPanelItem = sender as StackPanel;
            stackPanelItem.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));

        }

        private void ChooseAccess(string role)
        {
            //Only users with role SCM and role Administrator have total access
            if (role == Roles.SCM)
            {

            }
            else if (role == Roles.Administrator)
            {

            }
            else
            {
                //Users with access restrited only does can access the Query and Listing menu

            }
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
            menuQueries.Add(new SubItem("Funcionários", new Uri("Frames/Query/QueryUsers.xaml", UriKind.Relative)));

            var menuListingItem = new List<SubItem>();
            menuListingItem.Add(new SubItem("Inventário Oficial", new Uri("Frames/Listing/InventoryOfficer.xaml", UriKind.Relative)));
            menuListingItem.Add(new SubItem("Inventário Rotativo", new Uri("Frames/Listing/InventoryTurnover.xaml", UriKind.Relative)));
            menuListingItem.Add(new SubItem("Listagem de Permanentes", new Uri("Frames/Listing/ListPermanentProduct.xaml", UriKind.Relative)));
            //menuListingItem.Add(new SubItem("Financeiro (em breve)", new Uri("Frames/Listing/Financial.xaml", UriKind.Relative)));


            //var item0 = new ItemMenu("Painel de Controle", new UserControl(), MaterialDesignThemes.Wpf.PackIconKind.ViewDashboardOutline, new Uri("Frames/Usercontrol1.xaml", UriKind.Relative));
            var item1 = new ItemMenu("Movimentações", menuMovement, MaterialDesignThemes.Wpf.PackIconKind.ImportExport);
            var item2 = new ItemMenu("Cadastros", menuRegister, MaterialDesignThemes.Wpf.PackIconKind.AddCircleOutline);
            var item3 = new ItemMenu("Consultas", menuQueries, MaterialDesignThemes.Wpf.PackIconKind.Search);
            var item4 = new ItemMenu("Relatórios", menuListingItem, MaterialDesignThemes.Wpf.PackIconKind.BooksVariant);
            var item5 = new ItemMenu("Gestão de Usuários", new UserControl(), MaterialDesignThemes.Wpf.PackIconKind.People, new Uri("Frames/UserManager/UserManager.xaml", UriKind.Relative));

            //var s1 = new UserControlMenuItem(item0, this.FrameContent);
            //s1.FrameChanged += FrameChanged;
            //Menu.Children.Add(s1);

            var s1 = new UserControlMenuItem(item1, this.FrameContent);
            s1.FrameChanged += FrameChanged;
            Menu.Children.Add(s1);
            var s2 = new UserControlMenuItem(item2, this.FrameContent);
            s2.FrameChanged += FrameChanged;
            Menu.Children.Add(s2);
            var s3 = new UserControlMenuItem(item3, this.FrameContent);
            s3.FrameChanged += FrameChanged;
            Menu.Children.Add(s3);
            var s4 = new UserControlMenuItem(item4, this.FrameContent);
            s4.FrameChanged += FrameChanged;
            Menu.Children.Add(s4);
            var s5 = new UserControlMenuItem(item5, this.FrameContent);
            s5.FrameChanged += FrameChanged;
            Menu.Children.Add(s5);
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
                if (context.Header.Contains("Gestão"))
                {
                    if (userControlMenuItem.listViewMenu.SelectedIndex != -1)
                    {
                        userControlMenuItem.listViewMenu.SelectedIndex = -1;
                    }
                }
            }
        }

        #endregion
        private void InitializeNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipTitle = "Controle de Materiais";
            notifyIcon.BalloonTipText = "Controle de Materiais";
            notifyIcon.Text = "Controle de Materiais";
            notifyIcon.Visible = true;
            //var IcoPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "DETELNotify.ico");
            var IcoUri = new Uri("DETELNotify.ico", UriKind.Relative);
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }
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

        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.PopoutNotification.StaysOpen = false;
            }
            e.Handled = true;

        }
    }
}
