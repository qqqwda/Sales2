using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MainViewModel
    {

        #region Singleton 
        //se utiliza para no volver a instanciar la MVM
        //cada vez que se llama, y retorna la MVM que está
        //en memoria
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion


        #region Properties
        public LoginViewModel Login { get; set; }
        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }
        public MyUserASP UserASP { get; set; }

        public RegisterViewModel Register { get; set; }

        public ProductsViewModel Products { get; set; } //atributo Products
        public AddProductViewModel AddProduct { get; set; } 
        #endregion
        public EditProductViewModel EditProduct { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        #region Constructor

        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.LoadMenu();
            //this.Products = new ProductsViewModel();//Se instancia aqui porque aquí arranca la app
                                                    // this.AddProduct = new AddProductViewModel(); No es buena practica instanciar AddProduct cuando se ejecuta la app
        }

        
        #endregion

        #region Commands
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }
        #endregion

        #region Methods
        private async void GoToAddProduct()
        {

            this.AddProduct = new AddProductViewModel();//Esta AddProductViewModel se instancia cuando el usuario hace click en el command
            
            await App.Navigator.PushAsync(new AddProductPage());

            return;
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {

            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "AboutPage",
                Title = "About",
            });
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "SetupPage",
                Title = "Setup",
            });
            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "LoginPage",
                Title = "Exit",
            });




        }
        #endregion

    }
}
