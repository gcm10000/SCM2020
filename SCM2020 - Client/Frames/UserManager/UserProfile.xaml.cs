﻿using System;
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
        private List<ModelsLibraryCore.Business> Businesses;

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


            int indexPosition = Array.IndexOf(Enum.GetValues(typeof(PositionInSector)), InfoUser.Position);
            this.ComboBoxPosition.SelectedIndex = indexPosition;

            

            this.TextBoxRegister.Text = InfoUser.Register;
            this.TextBoxName.Text = InfoUser.Name;
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
            var register = TextBoxRegister.Text;
            var name = TextBoxName.Text;
            var business = ComboBoxBusiness.SelectedItem as Business;
            int? businessId = (business != null) ? (int?)business.Id : null;
            Sector sector = ComboBoxSector.SelectedItem as Sector;
            var sectorId = sector.Id;
            dynamic positionEnum = ComboBoxPosition.SelectedItem;
            int positionIndex = positionEnum.Value;

            Task.Run(() => UpdateProfile(InfoUser.Id, register, name, businessId, sectorId, positionIndex));
        }

        private void UpdateProfile(string id, string register, string name, int? businessId, int sectorId, int positionIndex)
        {
            Profile signUp = new Profile() 
            {
                Id = id,
                Register = register, 
                Name = name, 
                Sector = sectorId, 
                Business = businessId, 
                Position = (PositionInSector) positionIndex 
            };

            var result = APIClient.PostData(new Uri(Helper.ServerAPI, "user/UpdateProfile").ToString(), signUp, Helper.Authentication);
            MessageBox.Show(result, "Servidor diz:");
        }
    }
}
