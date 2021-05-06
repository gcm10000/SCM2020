using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SCM2020___Client.Frames.UserManager
{
    /// <summary>
    /// Lógica interna para EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        private string imagePath = string.Empty;
        private InfoUser InfoUser;
        private List<ModelsLibraryCore.Sector> Sectors;

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
            var ComboBoxSectors = Sectors.Select(x => x.NameSector);
            this.SectorComboBox.ItemsSource = ComboBoxSectors;

            this.RegisterTextBox.Text = InfoUser.Register;
            this.NameTextBox.Text = InfoUser.Name;
            this.BusinessTextBox.Text = InfoUser.ThirdParty;
            this.PositionComboBox.Text = InfoUser.Position.ToString();
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
            MessageBox.Show(this.GridImage.ActualWidth.ToString());
        }

        private void ButtonUpdateProfile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
