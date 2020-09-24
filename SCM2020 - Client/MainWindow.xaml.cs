using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
                    Name = "Gabriel " + millis
                }.ToJson();

                var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:52991/notify?user=" + user)
                .Build();
                connection.StartAsync().Wait();

                //send
                //connection.InvokeCoreAsync("notify", new[] {   } });
                connection.On("Receive", (object sender, object message) =>
                {
                    //Console.WriteLine($"{message.Sender.Key} to {message.Destination}: {message.Data}{Environment.NewLine}");
                });

                connection.On("notify", (string message) => 
                {
                    MessageBox.Show(message);
                });
            });
            Helper.MyWebBrowser = WebBrowser;

            ////Only users with role SCM and role Administrator have total access
            //if (!((Helper.Role == ModelsLibraryCore.Roles.Administrator) || (Helper.Role == ModelsLibraryCore.Roles.SCM)))
            //{
            //    //Users with access restrited only does can access the Query and Listing menu
            //    this.MovementItem.Visibility = Visibility.Collapsed;
            //    this.RegisterItem.Visibility = Visibility.Collapsed;
            //}
        }

        private void SignIn()
        {
            var signIn = APIClient.MakeSignIn(new Uri(Helper.Server, "user/login/").ToString(),
    Registration: "59450",
    IsPJERJRegistration: true,
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
    }
}
