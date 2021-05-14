using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Microsoft.Win32;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using WebAssemblyLibrary;

namespace SCM2020___Client.Frames
{
    /// <summary>
    /// Lógica interna para UpdateMaterial.xaml
    /// </summary>
    public partial class UpdateMaterial : Window
    {
        public class ResponseMaterial
        {
            public bool Ok { get; set; }
            public string Message { get; set; }
        }
        public bool Successful = false;
        public ConsumptionProduct Product { get; private set; }
        public UpdateMaterial()
        {
            InitializeComponent();

        }

        public UpdateMaterial(ConsumptionProduct product)
        {
            InitializeComponent();
            Product = product;
            this.CodeTextBox.Text = product.Code.ToString();
            this.DescriptionTextBox.Text = product.Description;

            Uri groupUri = new Uri(Helper.ServerAPI, $"group/");
            var groups = APIClient.GetData<List<ModelsLibraryCore.Group>>(groupUri.ToString());
            var nameGroups = groups.Select(x => x.GroupName).ToList();
            this.GroupComboBox.ItemsSource = nameGroups;
            this.GroupComboBox.SelectedIndex = this.GroupComboBox.Items.IndexOf(groups.Single(x => x.Id == product.Group).GroupName);
            this.LocalizationTextBox.Text = product.Localization;
            this.MininumStockTextBox.Text = product.MininumStock.ToString();
            this.MaximumStockTextBox.Text = product.MaximumStock.ToString();
            //this.NumberLocalizationTextBox.Text = product.NumberLocalization.ToString();
            this.StockTextBox.Text = product.Stock.ToString();
            this.UnityTextBox.Text = product.Unity;

        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {
            var photopath = this.ImageTextBox.Text;
            Product = new ConsumptionProduct()
            {
                Id = Product.Id,
                Code = int.Parse(CodeTextBox.Text),
                Description = DescriptionTextBox.Text,
                Localization = LocalizationTextBox.Text,
                MininumStock = double.Parse(MininumStockTextBox.Text),
                MaximumStock = double.Parse(MaximumStockTextBox.Text),
                //NumberLocalization = uint.Parse(NumberLocalizationTextBox.Text),
                Stock = double.Parse(StockTextBox.Text),
                Unity = UnityTextBox.Text,
                Group = (GroupComboBox.SelectedIndex + 1),
                Photo = (photopath != string.Empty) ? string.Concat(Product.Id, System.IO.Path.GetExtension(photopath)) : null
            };

            Task.Run(() => 
            {
                if (Product.Photo != null)
                {
                    var response = UploadImage(new Uri(Helper.ServerAPI, "generalproduct/UploadImage").ToString(), Product.Id, photopath);
                    if (response.Ok)
                    {
                        Successful = true;
                        Task.Run(() =>
                        {
                            UpdateProduct();
                        });
                    }
                    else
                    {
                        MessageBox.Show(response.Message, "Erro ao enviar imagem", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    Successful = true;
                    Task.Run(() =>
                    {
                        UpdateProduct();
                    });
                }
            });
        }

        private void UpdateProduct()
        {
            var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"generalproduct/update/{Product.Id}"), Product, Helper.Authentication);
            this.Dispatcher.Invoke(new Action(() => { this.DialogResult = true; }));
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Dispatcher.Invoke(new Action(() => { this.Close(); }));
            }

        }

        private void ImageTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ImageDialog();
        }

        private ResponseMaterial UploadImage(string url, int productId, string pathImage)
        {
            using (var client = new HttpClient())
            {
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new FileStream(pathImage, FileMode.Open)), "Image", System.IO.Path.GetFileName(pathImage));
                    content.Add(new StringContent(productId.ToString()), "ProductId");

                    using (var message = client.PostAsync(url, content).Result)
                    {
                        var response = message.Content.ReadAsStringAsync().Result;
                        return new ResponseMaterial() { Ok = message.IsSuccessStatusCode, Message = response };
                    }
                }
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            ImageDialog();
        }
        private void ImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Arquivos de imagem (*.png, *.gif, *.jpg, *.jpeg, *.bmp)|*.png;*.gif;*.jpg;*.jpeg;*.bmp;";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                this.ImageTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
