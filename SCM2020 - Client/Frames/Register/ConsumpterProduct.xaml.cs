using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Product.xam
    /// </summary>
    public partial class ConsumpterProduct : UserControl
    {
        public class InputImage
        {
            public int PhotoId { get; set; }
            public int Uid { get; set; }
            public TextBox TextBoxPath { get; set; }
            public bool NewImage { get; set; }
            public bool DeleteImage { get; set; }
            public InputImage(int Id, int Uid, TextBox TextBoxPath, bool NewImage)
            {
                this.PhotoId = Id;
                this.Uid = Uid;
                this.TextBoxPath = TextBoxPath;
                this.NewImage = NewImage;
            }
            public InputImage(int Uid, TextBox TextBoxPath, Button ButtonNewImage, bool NewImage)
            {
                this.Uid = Uid;
                this.TextBoxPath = TextBoxPath;
                this.NewImage = NewImage;
            }
        }
        List<InputImage> InputImages = new List<InputImage>();
        int RemoveImages = 0;

        public ConsumptionProduct Product { get; private set; }
        public bool UpdateProduct = false;
        public class ResponseMaterial
        {
            public bool Ok { get; set; }
            public Result Result { get; set; }
        }
        public ConsumpterProduct()
        {
            InitializeComponent();
            Uri groupUri = new Uri(Helper.ServerAPI, "group/");
            var groups = APIClient.GetData<List<ModelsLibraryCore.Group>>(groupUri.ToString());
            var nameGroups = groups.Select(x => x.GroupName).ToList();
            this.GroupComboBox.ItemsSource = nameGroups;
            RequestCodeAvaliable();
        }
        public ConsumpterProduct(ConsumptionProduct product)
        {
            UpdateProduct = true;
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
            this.StockTextBox.Text = product.Stock.ToString();
            this.UnityTextBox.Text = product.Unity;

            foreach (var photo in Product.Photos)
            {
                AddGridImageGUI(false, photo.Path, photo.Id);
            }
        }

        private void RequestCodeAvaliable()
        {
            Uri nextNumberUri = new Uri(Helper.ServerAPI, "generalproduct/nextnumber");
            var number = APIClient.GetData<int>(nextNumberUri.ToString(), Helper.Authentication);

            this.CodeTextBox.Text = number.ToString();
        }

        private void BtnConsumpterProduct_Click(object sender, RoutedEventArgs e)
        {

            ModelsLibraryCore.ConsumptionProduct consumptionProduct = new ModelsLibraryCore.ConsumptionProduct()
            {
                //(photopath != string.Empty) ? string.Concat(Product.Id, System.IO.Path.GetExtension(photopath)) : null
                Code = int.Parse(CodeTextBox.Text),
                Description = DescriptionTextBox.Text,
                Group = (GroupComboBox.SelectedIndex + 1),
                Localization = LocalizationTextBox.Text,
                MininumStock = double.Parse(MininumStockTextBox.Text),
                MaximumStock = double.Parse(MaximumStockTextBox.Text),
                Stock = double.Parse(StockTextBox.Text),
                Unity = UnityTextBox.Text,
                Photos = null //editar para update de produto
            };
            new Task(() =>
            {
                //Caso seja produto novo
                if (!UpdateProduct)
                {
                    var resultAddProduct = APIClient.PostData(new Uri(Helper.ServerAPI, "generalProduct/Add/"), consumptionProduct, Helper.Authentication);
                    if (resultAddProduct.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show(JsonConvert.DeserializeObject<string>(resultAddProduct.Result), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
                        if (resultAddProduct.Result.Contains("utilizado"))
                        {
                            System.Windows.Forms.DialogResult messageBoxResult = (System.Windows.Forms.DialogResult)MessageBox.Show("O SKU se encontra utilizado. Gostaria cadastrar este produto utilizando outro SKU?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                CodeTextBox.Dispatcher.Invoke(() => { RequestCodeAvaliable(); });
                                MessageBox.Show("SKU alterado. Por favor, tente novamente o cadastro do produto.");
                            }
                        }
                    }
                    else
                    {
                        var resultData = resultAddProduct.Result.DeserializeJson<Result>();
                        MessageBox.Show(resultData.Message, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                        foreach (var inputImage in InputImages)
                        {
                            if (inputImage.NewImage)
                            {
                                string TextBoxPathText = string.Empty;
                                inputImage.TextBoxPath.Dispatcher.Invoke(() => { TextBoxPathText = inputImage.TextBoxPath.Text; });
                                if (TextBoxPathText == string.Empty)
                                    return;
                                var resultUploadImage = UploadImage(new Uri(Helper.ServerAPI, "generalproduct/UploadImage").ToString(), resultData.Id ?? default(int), TextBoxPathText);
                                MessageBox.Show(resultUploadImage.Result.Message, "Servidor diz:", MessageBoxButton.OK, (resultUploadImage.Ok) ? MessageBoxImage.Information : MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else //Caso um produto seja atualizado
                {
                    var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"generalproduct/update/{Product.Id}"), consumptionProduct, Helper.Authentication);
                    //this.Dispatcher.Invoke(new Action(() => { this.DialogResult = true; }));
                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    var a = InputImages.Where(x => (x.NewImage == false) && (x.DeleteImage == true)); //Lista de imagens solicitadas
                    foreach (var item in a)
                    {
                        //Apagar imagens já adicionadas
                        var resultRemoveImage = APIClient.DeleteData(new Uri(Helper.ServerAPI, $"generalproduct/RemoveImage/{Product.Id}/{item.PhotoId}").ToString(), Helper.Authentication);
                        var deserializeJson = resultRemoveImage.DeserializeJson<Result>();
                        MessageBox.Show(deserializeJson.Message, $"Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    var b = InputImages.Where(x => (x.NewImage == true)); //Adicionar imagens 
                    foreach (var item in b)
                    {
                        string TextBoxPathText = string.Empty;
                        item.TextBoxPath.Dispatcher.Invoke(() => { TextBoxPathText = item.TextBoxPath.Text; });
                        if (TextBoxPathText == string.Empty)
                            return;
                        var resultUploadImage = UploadImage(new Uri(Helper.ServerAPI, "generalproduct/UploadImage").ToString(), Product.Id, TextBoxPathText);
                        MessageBox.Show(resultUploadImage.Result.Message, $"Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }).Start();
        }

        private void BtnAddImages_Click(object sender, RoutedEventArgs e)
        {
            AddGridImageGUI(true, string.Empty, 0);
        }
        private void AddGridImageGUI(bool NewImage, string TextBoxText, int PhotoId)
        {
            var numbers = InputImages.Select(x => x.Uid);
            int nextNumber = NextAvaliable(numbers);
            GridInputs.RowDefinitions.Add(new RowDefinition());
            Grid gridNewImage = new Grid()
            {
                Margin = new Thickness(10, 10, 10, 10),
                Uid = nextNumber.ToString()
            };

            gridNewImage.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) }); //textBoxPath
            gridNewImage.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto }); //buttonSelectImage
            gridNewImage.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto }); //buttonRemoveImage

            TextBox textboxImage = new TextBox()
            {
                Cursor = Cursors.Arrow,
                IsReadOnly = true,
                Style = null,
                Height = 30,
                FontSize = 16,
                Margin = new Thickness(10, 10, 10, 10),
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDCDCD"),
                Text = TextBoxText
            };
            textboxImage.SetValue(Grid.ColumnProperty, 0);

            if (NewImage)
            {
                textboxImage.MouseDoubleClick += (args, e) =>
                {
                    string fullFileName = ImageDialog();
                    if (fullFileName != null)
                    {
                        textboxImage.Text = fullFileName;
                    }
                };
            }

            System.Windows.Controls.Button buttonSelectImage = new System.Windows.Controls.Button()
            {
                Margin = new Thickness(0, 10, 5, 10),
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF1368BD"),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF"),
                Height = 30,
                SnapsToDevicePixels = true,
                BorderThickness = new Thickness(0),
                Content = "Selecionar Imagem",
                Visibility = (NewImage) ? Visibility.Visible : Visibility.Collapsed
            };
            buttonSelectImage.Click += (sender, e) =>
            {
                string fullFileName = ImageDialog();
                if (fullFileName != null)
                {
                    textboxImage.Text = fullFileName;
                }
            };

            System.Windows.Controls.Button buttonRemoveImage = new System.Windows.Controls.Button()
            {
                Margin = new Thickness(0, 10, 5, 10),
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF1368BD"),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF"),
                Height = 30,
                SnapsToDevicePixels = true,
                BorderThickness = new Thickness(0),
                Content = "Remover Imagem",
                Uid = nextNumber.ToString()
            };

            buttonRemoveImage.Click += (sender, e) =>
            {
                Button button = sender as Button;

                var gridInputsParent = gridNewImage.Parent as Grid;
                var inputImagesCurrent = InputImages.Single(x => x.Uid.ToString() == button.Uid);
                if (NewImage)
                {
                    InputImages.Remove(inputImagesCurrent);
                }
                else
                {
                    inputImagesCurrent.DeleteImage = true;
                    inputImagesCurrent.PhotoId = PhotoId;
                }

                gridNewImage.Children.Remove(textboxImage);
                gridNewImage.Children.Remove(buttonSelectImage);
                gridNewImage.Children.Remove(button);
                gridInputsParent.Children.Remove(gridNewImage);
                RemoveImages++;
            };

            buttonSelectImage.SetValue(Grid.ColumnProperty, 1);
            buttonRemoveImage.SetValue(Grid.ColumnProperty, 2);
            gridNewImage.Children.Add(textboxImage);
            gridNewImage.Children.Add(buttonSelectImage);
            gridNewImage.Children.Add(buttonRemoveImage);
            gridNewImage.SetValue(Grid.RowProperty, GridInputs.Children.Count + RemoveImages);
            GridInputs.Children.Add(gridNewImage);
            InputImages.Add(new InputImage(nextNumber, textboxImage, buttonSelectImage, NewImage));
        }
        private int NextAvaliable(IEnumerable<int> myInts)
        {
            int firstAvailable = Enumerable.Range(1, Int32.MaxValue).Except(myInts).First();
            return firstAvailable;
        }
        private string ImageDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Arquivos de imagem (*.png, *.gif, *.jpg, *.jpeg, *.bmp)|*.png;*.gif;*.jpg;*.jpeg;*.bmp;";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                return openFileDialog.FileName;
            }
            return null;
        }
        private ResponseMaterial UploadImage(string url, int productId, string pathImage)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = Helper.Authentication;
                    using (var content =
                        new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                    {
                        content.Add(new StreamContent(new FileStream(pathImage, FileMode.Open)), "Image", System.IO.Path.GetFileName(pathImage));
                        content.Add(new StringContent(productId.ToString()), "ProductId");

                        using (var message = client.PostAsync(url, content).Result)
                        {
                            string response = message.Content.ReadAsStringAsync().Result;
                            return new ResponseMaterial() { Ok = message.IsSuccessStatusCode, Result = response.DeserializeJson<Result>() };
                        }
                    }
                }
                catch (System.UnauthorizedAccessException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }

    }
}
