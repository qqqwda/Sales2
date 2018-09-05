using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Services;
using Sales.Views;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;

        #endregion

        #region Constructor
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion
        #region Commands
        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        

        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        

        public ICommand SelectProductCommand
        {
            get
            {
                return new RelayCommand(SelectProduct);
            }
        }
        #endregion
        #region Methods

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("warning", "Are you sure you want to delete this product?", "No", "Confirm");
            if (answer)
            {
                return;
            }
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }

            var response = await this.apiService.Delete("http://10.0.4.113", "/SalesApi/api", "/Products", this.ProductId);
            if (!response.IsSuccess)
            {
                
                await Application.Current.MainPage.DisplayAlert("Error",response.Message,"Accept");
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();
            if(deletedProduct != null)
            {
                productsViewModel.Products.Remove(deletedProduct);
            }

        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }

        private async void SelectProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        } 
        #endregion
    }
}
