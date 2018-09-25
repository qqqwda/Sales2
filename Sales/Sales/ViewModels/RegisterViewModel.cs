using GalaSoft.MvvmLight.Command;
using Plugin.Media.Abstractions;
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

        public string ConfirmPassword { get; set; }

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
            if (string.IsNullOrEmpty(this.FirstName))
            {
                Error("First name is empty", "Accept");
                return;
            }
            if (!this.Password.Equals(this.ConfirmPassword))
            {
                Error("Passwords doesn't match", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Address))
            {
                Error("Address invalid", "Accept");
                return;
            }
            if (!RegexHelper.IsValidEmailAddress(this.EMail))
            {
                Error("Invalid Email","Accept");
                return;
            }
            if (string.IsNullOrEmpty(this.Phone))
            {
                Error("Invalid phone","Accept");
                return;
            }
        }

        private async void Error(string v2, string v3)
        {
            await Application.Current.MainPage.DisplayAlert("Error", v2, v3);
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
        #endregion
    }
}
