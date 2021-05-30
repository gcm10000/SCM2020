using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SCM2020___Client.Frames.Register
{
    /// <summary>
    /// Interação lógica para Product.xam
    /// </summary>
    public partial class ConsumpterProduct : UserControl
    {
        public ConsumptionProduct Product { get; private set; }
        List<string> fullNamePhotos { get; set; }

        public ConsumpterProduct()
        {
            InitializeComponent();
            fullNamePhotos = new List<string>();
            Uri groupUri = new Uri(Helper.ServerAPI, "group/");
            var groups = APIClient.GetData<List<ModelsLibraryCore.Group>>(groupUri.ToString());
            var nameGroups = groups.Select(x => x.GroupName).ToList();
            this.GroupComboBox.ItemsSource = nameGroups;
            RequestCodeAvaliable();
        }
        public ConsumpterProduct(ConsumptionProduct product)
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
                Photos = null
            };
            new Task(() =>
            {
                var result = APIClient.PostData(new Uri(Helper.ServerAPI, "generalProduct/Add/"), consumptionProduct, Helper.Authentication);
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(result.Result), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (result.Result.Contains("utilizado"))
                    {
                        System.Windows.Forms.DialogResult messageBoxResult = (System.Windows.Forms.DialogResult)MessageBox.Show("O SKU se encontra utilizado. Gostaria cadastrar este produto utilizando outro SKU?", "Pergunta", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (messageBoxResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            CodeTextBox.Dispatcher.Invoke(new Action(() => { RequestCodeAvaliable(); }));
                            MessageBox.Show("SKU alterado. Por favor, tente novamente o cadastro do produto.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(JsonConvert.DeserializeObject<string>(result.Result), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }).Start();
        }

        private void BtnAddImages_Click(object sender, RoutedEventArgs e)
        {

            GridInputs.RowDefinitions.Add(new RowDefinition());
            Grid gridNewImage = new Grid()
            {
                Margin = new Thickness(10, 10, 10, 10)
            };
            gridNewImage.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            gridNewImage.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            
            TextBox textboxImage = new TextBox()
            {
                Cursor = Cursors.Arrow,
                IsReadOnly = true,
                Style = null,
                Height = 30,
                FontSize = 16,
                Margin = new Thickness(10, 10, 10, 10),
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#CDCDCD")
            };
            textboxImage.SetValue(Grid.ColumnProperty, 0);

            textboxImage.MouseDoubleClick += (args, e) => 
            {
                string fullFileName = ImageDialog();
                if (fullFileName != null)
                {
                    textboxImage.Text = fullFileName;
                    fullNamePhotos.Add(fullFileName);
                }

            };

            System.Windows.Controls.Button buttonSelectImage = new System.Windows.Controls.Button() 
            {
                Margin = new Thickness(0, 10, 5, 10),
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF1368BD"),
                Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF"),
                Height = 30,
                SnapsToDevicePixels = true,
                BorderThickness = new Thickness(0),
                Content = "Selecionar Imagem"
            };
            buttonSelectImage.Click += (sender, e) => 
            {
                string fullFileName = ImageDialog();
                if (fullFileName != null)
                {
                    textboxImage.Text = fullFileName;
                    fullNamePhotos.Add(fullFileName);
                }
            };
            buttonSelectImage.SetValue(Grid.ColumnProperty, 1);
            
            gridNewImage.Children.Add(textboxImage);
            gridNewImage.Children.Add(buttonSelectImage);
            gridNewImage.SetValue(Grid.RowProperty, GridInputs.Children.Count);
            GridInputs.Children.Add(gridNewImage);

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
    }
}
