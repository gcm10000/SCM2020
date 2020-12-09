using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
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
        System.Windows.Forms.NotifyIcon notifyIcon1;
        public MainWindow()
        {
            notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            notifyIcon1.Icon = System.Drawing.SystemIcons.Exclamation;
            notifyIcon1.BalloonTipTitle = "Sistema de Controle de Materiais";
            notifyIcon1.Visible = true;

            InitializeComponent();
            PopupMovement.VerticalOffset = -8;
            PopupRegister.VerticalOffset = -8;
            PopupQueries.VerticalOffset = -8;
            PopupReport.VerticalOffset = -8;

            PopupMovement.HorizontalOffset = 21;
            PopupRegister.HorizontalOffset = 76;
            PopupQueries.HorizontalOffset = 70;
            PopupReport.HorizontalOffset = 68;

            //LoginScreen screen = new LoginScreen();
            //screen.ShowDialog();

            //SignIn();

            //var t = new System.Windows.Forms.TreeView();



            WebAssemblyLibrary.Helper.SetLastVersionIE();
            Task.Run(() =>
            {
                Helper.WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder().Build();
                Helper.WebHost.Run();
                //Default url is http://localhost:5000/
            });

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
                .WithUrl("http://localhost:52991/notify?user=" + user)
                .Build();

                //send
                //connection.InvokeCoreAsync("notify", new[] {   } });
                connection.On("Receive", (object sender, object message) =>
                {
                    //Console.WriteLine($"{message.Sender.Key} to {message.Destination}: {message.Data}{Environment.NewLine}");
                });

                bool initialize = false;
                connection.On("notify", (string stockMessageJson) =>
                {
                    AlertStockMessage stockMessage = stockMessageJson.DeserializeJson<AlertStockMessage>();
                    notifyIcon1.BalloonTipText = stockMessage.Message;
                    notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                    if (!initialize)
                    {
                        initialize = true;
                        notifyIcon1.BalloonTipClicked += (object sender, EventArgs e) =>
                        {
                            //abrir janela do estoque exibindo o produto
                            MessageBox.Show(stockMessage.Code.ToString());
                        };
                    }
                    notifyIcon1.ShowBalloonTip(30000);
                });

                connection.StartAsync().Wait();
            });
            Helper.MyWebBrowser = WebBrowser;

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            notifyIcon1.Dispose();
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
        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var index = listView.SelectedIndex;
            var item = listView.SelectedItem as ListViewItem;
            if (item == null)
                return;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                var lvItem = listView.Items[i] as ListViewItem;
                lvItem.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                lvItem.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                lvItem.IsSelected = false;
            }
            item.Background = new SolidColorBrush(Color.FromRgb(0xE9, 0xED, 0xFF));
            item.Foreground = new SolidColorBrush(Color.FromRgb(0x4F, 0x68, 0xFF));

            switch (index)
            {
                case 1:
                    PopupMovement.IsOpen = true;
                    break;
                case 2:
                    PopupRegister.IsOpen = true;
                    break;
                case 3:
                    PopupQueries.IsOpen = true;
                    break;
                case 4:
                    PopupReport.IsOpen = true;
                    break;
                //case 5:
                //    PopupUser.IsOpen = true;
                //    break;
                default:
                    break;
            }
        }
        private void ListViewMoving_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;

            if (item != null)
            {
                Uri source = null;
                switch (index)
                {
                    case 0:
                        source = new Uri("Frames/Movement/InputByVendor.xaml", UriKind.Relative);
                        break;
                    case 1:
                        source = new Uri("Frames/Movement/Devolution.xaml", UriKind.Relative);
                        break;
                    case 2:
                        source = new Uri("Frames/Movement/MaterialOutput.xaml", UriKind.Relative);
                        break;
                    case 3:
                        source = new Uri("Frames/Movement/Closure.xaml", UriKind.Relative);
                        break;
                    case 4:
                        source = new Uri("Frames/Movement/Reopen.xaml", UriKind.Relative);
                        break;
                }
                PopupMovement.IsOpen = false;
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

        }
        private void ListBoxRegister_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                Uri source = null;
                switch (index)
                {
                    case 0:
                        source = new Uri("Frames/Register/Employee.xaml", UriKind.Relative);
                        break;
                    case 1:
                        source = new Uri("Frames/Register/ConsumpterProduct.xaml", UriKind.Relative);
                        break;
                    case 2:
                        source = new Uri("Frames/Register/PermanentProduct.xaml", UriKind.Relative);
                        break;
                    case 3:
                        source = new Uri("Frames/Register/Group.xaml", UriKind.Relative);
                        break;
                    case 4:
                        source = new Uri("Frames/Register/Vendor.xaml", UriKind.Relative);
                        break;
                    case 5:
                        source = new Uri("Frames/Register/Sector.xaml", UriKind.Relative);
                        break;
                    case 6:
                        source = new Uri("Frames/Register/Business.xaml", UriKind.Relative);
                        break;
                }
                PopupRegister.IsOpen = false;
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
        }
        private void ListBoxQueries_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                Uri source = null;
                switch (index)
                {
                    case 0:
                        source = new Uri("Frames/Query/Movement.xaml", UriKind.Relative);
                        break;
                    case 1:
                        source = new Uri("Frames/Query/InputByVendor.xaml", UriKind.Relative);
                        break;
                    case 2:
                        source = new Uri("Frames/Query/StockQuery.xaml", UriKind.Relative);
                        break;
                    case 3:
                        source = new Uri("Frames/Query/QueryByDate.xaml", UriKind.Relative);
                        break;
                    case 4:
                        source = new Uri("Frames/Query/QueryByPatrimony.xaml", UriKind.Relative);
                        break;
                    case 5:
                        source = new Uri("Frames/Query/QueryWorkOrderByDate.xaml", UriKind.Relative);
                        break;
                    case 6:
                        source = new Uri("Frames/Query/QueryUsers.xaml", UriKind.Relative);
                        break;
                }
                PopupQueries.IsOpen = false;
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
        }
        private void ListBoxReport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListBox;
            var item = listView.SelectedItem as ListBoxItem;
            var index = listView.SelectedIndex;
            if (item != null)
            {
                Uri source = null;
                switch (index)
                {
                    case 0:
                        source = new Uri("Frames/Listing/InventoryOfficer.xaml", UriKind.Relative);
                        break;
                    case 1:
                        source = new Uri("Frames/Listing/InventoryTurnover.xaml", UriKind.Relative);
                        break;
                    case 2:
                        source = new Uri("Frames/Listing/ListPermanentProduct.xaml", UriKind.Relative);
                        break;
                    case 3:
                        source = new Uri("Frames/Listing/Financial.xaml", UriKind.Relative);
                        break;

                }
                PopupReport.IsOpen = false;
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
        }

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
    }
}
