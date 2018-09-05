using GalaSoft.MvvmLight.Command;
using Sales.Views;
using System;
using System.Collections.Generic;
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

        public ProductsViewModel Products { get; set; } //atributo Products
        public AddProductViewModel AddProduct { get; set; } 
        #endregion
        public EditProductViewModel EditProduct { get; set; }
        #region Constructor

        public MainViewModel()
        {
            instance = this;
            this.Products = new ProductsViewModel();//Se instancia aqui porque aquí arranca la app
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
            
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());

            return;
        } 
        #endregion

    }
}
