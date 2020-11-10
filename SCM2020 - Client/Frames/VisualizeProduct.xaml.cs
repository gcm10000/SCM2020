using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Lógica interna para VisualizeProduct.xaml
    /// </summary>
    public partial class VisualizeProduct : Window
    {
        public VisualizeProduct()
        {
            InitializeComponent();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetNewImage();
        }
        private void GetNewImage()
        {
            string url = SearchUri("cabo espiral bege").ToString();
            var html = APIClient.GetData(url);
            //div class="mJxzWe"

            MessageBox.Show(html);
        }
        private Uri SearchUri(string query)
        {
            string url = @"https://www.google.com.br/search?tbm=isch&q=" + HttpUtility.UrlEncode(query);
            Uri uri = new Uri(url);
            return uri;
        }
    }
}
