using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Claims;
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
using System.Windows.Shapes;
using System.Security.Authentication;
using WebAssemblyLibrary;

namespace SCM2020___Client
{
    /// <summary>
    /// Lógica interna para LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {

        public LoginScreen()
        {

            InitializeComponent();
            UserTextBox.Focus();

            var treeview = new TreeView<KeyValuePair<CompanyPosition, List<Employee>>>();

            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode1 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Director, new List<Employee>() { new Employee("Coronel"), new Employee("Major") }));
            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode2 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Engineer, new List<Employee>() { new Employee("Luiz"), new Employee("Milene") }));
            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode3 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Engineer, new List<Employee>() { new Employee("Carlos"), new Employee("Ricardo")  }));
            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode6 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Manager, new List<Employee>() { new Employee("Cézar Gabriel") }));
            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode7 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Manager, new List<Employee>() { new Employee("Robson")  }));
            TreeNode<KeyValuePair<CompanyPosition, List<Employee>>> treeNode8 = new TreeNode<KeyValuePair<CompanyPosition, List<Employee>>>(new KeyValuePair<CompanyPosition, List<Employee>>(CompanyPosition.Supervisor, new List<Employee>() { new Employee("Daniel"), new Employee("Alex")  }));


            treeview.Nodes.Add(treeNode1);
            treeview.Nodes[0].Nodes.Add(treeNode2);
            treeview.Nodes[0].Nodes.Add(treeNode3);
            treeview.Nodes[0].Nodes[0].Nodes.Add(treeNode6);
            treeview.Nodes[0].Nodes[1].Nodes.Add(treeNode7);
            treeview.Nodes[0].Nodes[0].Nodes[0].Nodes.Add(treeNode8);
            //treeNode 8 -> 6 -> 2 -> 1
            treeNode8.IsDescendant(treeNode6);
            Console.WriteLine(treeview.ToJson());
            Console.WriteLine(treeview.GetAllNodes().ToJson());
        }



        private void SignInButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ConnectServer();
        }
        private void ConnectServer()
        {
            //user 59450
            //password SenhaSecreta#2020
            var user = UserTextBox.Text;
            var password = PasswordTextBox.Password;
            var t = Task.Run(() =>
            {
                if (SignIn(user, password))
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MainWindow window = new MainWindow();
                        window.Show();
                        this.Close();
                    });
                }
            });
        }
        private bool SignIn(string user, string password)
        {
            try
            {
                var signIn = APIClient.MakeSignIn(new Uri(Helper.ServerAPI, "user/login/").ToString(),
                    Register: user,
                    Password: password);

                Helper.Authentication = signIn.Headers.Authorization;
                Helper.NameIdentifier = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                Helper.CurrentSector = signIn.Sector;
                Helper.Role = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                //MessageBox.Show("Login realizado com sucesso.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (AggregateException ex)
            {
                //Messagebox será exibido quando não houver conectividade com o servidor.
                if (ex.InnerExceptions.Any(x => x.GetType().ToString() == "System.Net.Http.HttpRequestException"))
                {
                    MessageBox.Show("Não foi possível conectar ao servidor.", "Erro de conectividade", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            //Messagebox será exibido como uma resposta do servidor.
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
            //É preciso de outro catch para credenciais erradas.
            return false;
        }

        private void UserTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                ConnectServer();
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                ConnectServer();
        }
    }
}
