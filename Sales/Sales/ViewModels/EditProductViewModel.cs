using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class EditProductViewModel : BaseViewModel
    {
        private Product product;
        private MediaFile file;
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;

        #region MyRegion
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

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        } 
        #endregion

        #region Constructor
        public EditProductViewModel(Product product)
        {
            this.product = product;
            apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = product.ImageFullPath;
        }
        #endregion

        #region Commands
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

        #region Methods
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

        private async void Save()
        {
            this.IsEnabled = true;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Product.Description))
            {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Please, add a description", "Appect");
                return;
            }
            

            if (this.Product.Price <= 0)
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Please, add a positive price", "Appect");
                return;
            }

            this.IsEnabled = false;
            this.IsRunning = true;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }
            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FileHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }
            
            var response = await this.apiService.Put<Product>("http://10.0.4.113", "/SalesApi/api", "/Products", this.Product, this.Product.ProductId);
            if (!response.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var newProduct = (Product)response.Result; //response.Result devuelve tipo object
            var viewModel = ProductsViewModel.GetInstance();
            
            viewModel.Products.Add(ToProductItemViewModel(newProduct));


            await App.Navigator.PopAsync();


            //apiService.Post<>();
        }

        private ProductItemViewModel ToProductItemViewModel(Product n)
        {
            return new ProductItemViewModel
            {
                Description = n.Description,
                ImagePath = n.ImagePath,
                IsAvailable = n.IsAvailable,
                Price = n.Price,
                ProductId = n.ProductId,
                PublishOn = n.PublishOn,
                Remarks = n.Remarks,
            };

        }

        #endregion
    }
}
