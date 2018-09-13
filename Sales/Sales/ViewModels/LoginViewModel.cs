using GalaSoft.MvvmLight.Command;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos
        private bool isRunning;
        private bool isEnabled;
        private bool isRemembered;
        private ApiService apiService;
        #endregion
        #region Properties
        public String Email { get; set; }
        public String Password { get; set; }

        public bool IsRemembered
        {
            get { return this.isRemembered; }
            set { this.SetValue(ref this.isRemembered, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }


        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Ctor
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
        #endregion

        #region Methods
        private async void Login()
        {
            if (String.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an Email", "Accept");
                return;
            }
            if (String.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a password", "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", "Please turn on your internet settings", "Accept");
                return;
            }

            var token = await this.apiService.GetToken("http://10.0.4.113/SalesApi/Token", this.Email,this.Password);

            if(token == null || String.IsNullOrEmpty(token.AccessToken))
            {

                await Application.Current.MainPage.DisplayAlert("Error", "Password or username incorrect","Accept");
                return;
            }

            await Application.Current.MainPage.DisplayAlert("Ok","ok","ok");
        }

        private void ForgotPassword()
        {
            throw new NotImplementedException();
        }

        private void Register()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Command

        public ICommand LoginCommand
        { get
            {
                return new RelayCommand(Login);
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        

        public ICommand ForgotPasswordComand
        { get
            {
                return new RelayCommand(ForgotPassword);
            }
        }



        #endregion
    }
}
