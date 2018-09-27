using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Atributos

        private MediaFile file;

        private ImageSource imageSource;

        private ApiService apiService;

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region ctor

        public RegisterViewModel()
        {
            this.ImageSource = "nouser";
            this.IsEnabled = true;
            this.apiService = new ApiService();

        }

        #endregion

        #region Prop
        public string EMail { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }


        #endregion

        #region Methods
        private async void Save()
        {
            this.IsRunning = true;
            this.IsEnabled = false;
            if (string.IsNullOrEmpty(this.FirstName))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("First name is empty", "Accept");
                return;
            }
            if (!this.Password.Equals(this.PasswordConfirm))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("Passwords doesn't match", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 6)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("Password must have 6 digits", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Address))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("Address invalid", "Accept");
                return;
            }
            if (!RegexHelper.IsValidEmailAddress(this.EMail))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("Invalid Email","Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Phone))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error("Invalid phone","Accept");
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error(connection.Message, "Accept");
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FileHelper.ReadFully(this.file.GetStream());
            }

            var userRequest = new UserRequest{Address = this.Address, EMail = this.EMail, FirstName = this.FirstName, LastName=this.LastName, Phone=this.Phone, Password=this.Password, ImageArray=imageArray};
            string controller = Application.Current.Resources["UrlUsersController"].ToString();

            var response = await this.apiService.Post("http://10.0.4.113", "/SalesApi/api", controller, userRequest);

            if (!response.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                DisplayAlert.Error(response.Message, "Accept");
                return;
            }


            this.IsRunning = false;
            await Application.Current.MainPage.DisplayAlert("Confirmed","Now you can access with your Email","Accept");
            Application.Current.MainPage.Navigation.PopAsync();
            return;
        }




        


        #endregion

        #region ICommand

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        #endregion

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Image source",
                "Cancel",
                null,
                "From gallery",
                "New picture");

            if (source.Equals("Cancel"))
            {
                this.file = null;
                return;
            }
            if (source.Equals("New Picture"))
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,

                    }
                    );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.imageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }
    }
}
