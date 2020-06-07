using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        private void SignInButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //user 59450
            //password SenhaSecreta#2020
            var user = UserTextBox.Text;
            var password = PasswordTextBox.Password;
            try
            {
                var signIn = APIClient.MakeSignIn(new Uri(Helper.Server, "user/login/").ToString(),
                    Registration: user,
                    IsPJERJRegistration: true,
                    Password: password);
                Helper.Authentication = signIn.Authorization;
                MessageBox.Show("Login realizado com sucesso.", "Informação:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
