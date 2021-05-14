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
using System.Windows.Shapes;
using Microsoft.Win32;
using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using WebAssemblyLibrary;
using static SCM2020___Client.Frames.UpdateMaterial;

namespace SCM2020___Client.Frames.UserManager
{
    /// <summary>
    /// Lógica interna para EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        private string imagePath = string.Empty;
        private InfoUser InfoUser;
        private Profile Profile;
        private List<ModelsLibraryCore.Sector> Sectors;
        private List<ModelsLibraryCore.Business> Businesses;
        public bool Successful = false;


        public EditProfile(InfoUser infoUser)
        {
            this.InfoUser = infoUser;
            InitializeComponent();

            this.BorderImagemProfile.Background = Brushes.White;

            FillUI();
        }

        private void FillUI()
        {
            Sectors = APIClient.GetData<List<ModelsLibraryCore.Sector>>(new Uri(Helper.ServerAPI, "sector").ToString(), Helper.Authentication);
            Businesses = APIClient.GetData <List<ModelsLibraryCore.Business>>(new Uri(Helper.ServerAPI, "business").ToString(), Helper.Authentication);
            //var SectorsToComboBox = new List<string>();

            this.ComboBoxSector.ItemsSource = Sectors;
            this.ComboBoxBusiness.ItemsSource = Businesses;

            this.ComboBoxPosition.ItemsSource = Enum.GetValues(typeof(PositionInSector)).Cast<object>().Select(e => new { Value = (int)e, DisplayName = e.ToString() });
            this.ComboBoxPosition.DisplayMemberPath = "DisplayName";
            this.ComboBoxPosition.SelectedValuePath = "Value";

            int indexSector = Sectors.FindIndex(x => x.NameSector == InfoUser.Sector.NameSector);
            this.ComboBoxSector.SelectedIndex = indexSector;

            var businessName = (InfoUser.Business != null) ? InfoUser.Business.Name : string.Empty;
            int indexBusiness = Businesses.FindIndex(x => x.Name == businessName);
            this.ComboBoxBusiness.SelectedIndex = indexBusiness;


            int indexPosition = Array.IndexOf(Enum.GetValues(typeof(PositionInSector)), InfoUser.Position);
            this.ComboBoxPosition.SelectedIndex = indexPosition;
            

            this.TextBoxRegister.Text = InfoUser.Register;
            this.TextBoxName.Text = InfoUser.Name;

            BitmapImage img = new BitmapImage(new Uri(Helper.Server, InfoUser.Photo));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            this.BorderImagemProfile.Background = image;

            this.ButtonRemoveImage.IsEnabled = InfoUser.Photo != null;
        }

        private void ButtonEditImage_Click(object sender, RoutedEventArgs e)
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
                this.imagePath = openFileDialog.FileName;
                BitmapImage img = new BitmapImage(new Uri(this.imagePath));
                ImageBrush image = new ImageBrush();
                image.ImageSource = img;
                this.BorderImagemProfile.Background = image;
            }
        }

        private void ButtonRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            { 
                var result = APIClient.GetData<string>(new Uri(Helper.ServerAPI, $"User/RemoveImage/{InfoUser.Id}").ToString(), Helper.Authentication);
                MessageBox.Show(result, "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ButtonUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            var register = TextBoxRegister.Text;
            var name = TextBoxName.Text;
            var business = ComboBoxBusiness.SelectedItem as Business;
            int? businessId = (business != null) ? (int?)business.Id : null;
            Sector sector = ComboBoxSector.SelectedItem as Sector;
            var sectorId = sector.Id;
            dynamic positionEnum = ComboBoxPosition.SelectedItem;
            int positionIndex = positionEnum.Value;

            Profile = new Profile()
            {
                Id = InfoUser.Id,
                Register = register,
                Name = name,
                Sector = sectorId,
                Business = businessId,
                Position = (PositionInSector)positionIndex,
                Photo = (imagePath != string.Empty) ? string.Concat(InfoUser.Id, System.IO.Path.GetExtension(imagePath)) : null
            };

            Task.Run(() => 
            {
                if (Profile.Photo != null)
                {
                    var response = UploadImage(new Uri(Helper.ServerAPI, "User/UploadImage").ToString(), Profile.Id, imagePath);
                    if (response.Ok)
                    {
                        Successful = true;
                        Task.Run(() =>
                        {
                            UpdateProfile();
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
                        UpdateProfile();
                    });
                }
            });
        }

        private void UpdateProfile()
        {
            var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"user/UpdateProfile"), Profile, Helper.Authentication);
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

        private ResponseMaterial UploadImage(string url, string userId, string pathImage)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = Helper.Authentication;

                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new FileStream(pathImage, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)), "Image", System.IO.Path.GetFileName(pathImage));
                    content.Add(new StringContent(userId.ToString()), "UserId");

                    using (var message = client.PostAsync(url, content).Result)
                    {
                        var response = message.Content.ReadAsStringAsync().Result;
                        return new ResponseMaterial() { Ok = message.IsSuccessStatusCode, Message = response };
                    }
                }
            }
        }


        //private void UpdateProduct()
        //{
        //    var result = APIClient.PostData(new Uri(Helper.ServerAPI, $"user/update/{Product.Id}"), Product, Helper.Authentication);
        //    this.Dispatcher.Invoke(new Action(() => { this.DialogResult = true; }));
        //    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //    {
        //        MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    else
        //    {
        //        MessageBox.Show(result.Result.DeserializeJson<string>(), "Servidor diz:", MessageBoxButton.OK, MessageBoxImage.Information);
        //        this.Dispatcher.Invoke(new Action(() => { this.Close(); }));
        //    }

        //}
    }
}
