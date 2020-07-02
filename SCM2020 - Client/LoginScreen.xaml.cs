﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            var t = Task.Run(() => SignIn(user, password));
            //t.Wait();

        }
        private void SignIn(string user, string password)
        {
            try
            {
                var signIn = APIClient.MakeSignIn(new Uri(Helper.Server, "user/login/").ToString(),
                    Registration: user,
                    IsPJERJRegistration: true,
                    Password: password);

                Helper.Authentication = signIn.Headers.Authorization;
                Helper.NameIdentifier = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                Helper.CurrentSector = signIn.Sector;
                Helper.Role = signIn.JwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                MessageBox.Show("Login realizado com sucesso.", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Any(x => x.GetType().ToString() == "System.Net.Http.HttpRequestException"))
                {
                    MessageBox.Show("Não foi possível conectar ao servidor.", "Erro de conectividade", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
