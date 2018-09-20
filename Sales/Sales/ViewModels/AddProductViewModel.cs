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
    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes
        private MediaFile file;
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        #endregion

        #region Properties
        public string Description { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }

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

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion


        #region Constructors
        public AddProductViewModel()
        {
            apiService = new ApiService();
            this.isEnabled = true;
            this.ImageSource = "noproduct";
           
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
                        Name="test.jpg",
                        PhotoSize = PhotoSize.Small,
                        
                    }
                    );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if(this.file != null)
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
            if (string.IsNullOrEmpty(this.Description))
            {

                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Please, add a description", "Appect");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Please, add a price", "Appect");
                return;
            }

            try
            {
                var p = decimal.Parse(this.Price);
            }
            catch
            {
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", "Price format incorrect", "Appect");
                return;
            }
            var price = decimal.Parse(this.Price);

            if (price <= 0)
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
            }

            var product = new Product
            {

                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
                ImageArray = imageArray,
            };

            var response = await this.apiService.Post<Product>("http://10.0.4.113", "/SalesApi/api", "/Products", product);
            if (!response.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            //if (response.IsSuccess)
            //{
            //    await Application.Current.MainPage.DisplayAlert("","Done!","Ok");
            //}

            var newProduct = (Product)response.Result;
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
