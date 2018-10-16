using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        #region Attributes
        private string filter;

        private ApiService apiService;

        private bool isRefreshing;

        private ObservableCollection<CategoryItemViewModel> categories;
        #endregion

        #region Properties
        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Category> MyCategories { get; set; }

        public ObservableCollection<CategoryItemViewModel> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public CategoriesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadCategories();
        }
        #endregion

        #region Methods
        private async void LoadCategories()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var url = "http://10.0.4.113".ToString();
            var prefix = "/SalesApi/api".ToString();
            var controller = "/Categories".ToString();
            //var response = await this.apiService.GetList<Category>("http://10.0.4.113", "/SalesApi/api", "/Products", Settings.TokenType, Settings.AccessToken);
            var response = await this.apiService.GetList<Category>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            this.MyCategories = (List<Category>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        private void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListCategoriesItemViewModel = this.MyCategories.Select(c => new CategoryItemViewModel
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description,
                    ImagePath = c.ImagePath,
                });

                this.Categories = new ObservableCollection<CategoryItemViewModel>(
                    myListCategoriesItemViewModel.OrderBy(c => c.Description));
            }
            else
            {
                var myListCategoriesItemViewModel = this.MyCategories.Select(c => new CategoryItemViewModel
                {
                    CategoryId = c.CategoryId,
                    Description = c.Description,
                    ImagePath = c.ImagePath,
                }).Where(c => c.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Categories = new ObservableCollection<CategoryItemViewModel>(
                    myListCategoriesItemViewModel.OrderBy(c => c.Description));
            }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }
        #endregion
    }


}

