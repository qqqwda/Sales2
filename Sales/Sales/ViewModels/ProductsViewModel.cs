using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Atributos
        private bool isRefreshing;
        private ApiService apiService;
        private DataService dataService;
        private ObservableCollection<ProductItemViewModel> products;
        private List<Product> list;
        private String filter;

        #endregion


        #region Singleton 
        //se utiliza para no volver a instanciar la ProductsViewModel
        //cada vez que se llama, y retorna la ProductsViewModel que está
        //en memoria
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Properties
        public Category Category { get; set; }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public String Filter
        {
            get { return this.filter; }
            set
            {
                this.SetValue(ref this.filter, value);
                RefreshList();
            }
        }
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        } //atributo observableCollection de tipo Product(Common) llamada "Products" 
        #endregion

        #region Constructor
        public ProductsViewModel(Category category)
        {
            instance = this;
            this.Category = category;
            this.dataService = new DataService();
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region Methods
        //private async void LoadProducts()
        //{
        //    this.IsRefreshing = true;
        //    var connection = await this.apiService.CheckConnection();

        //    if (connection.IsSuccess)
        //    {
        //        var answer = await this.LoadProductsFromAPI();

        //        if (answer)
        //        {
        //            this.SaveProductsToDB();
        //        }
        //    }
        //    else
        //    {
        //        await this.LoadProductsFromDB();
        //    }

        //    if(this.list == null || this.list.Count == 0)
        //    {
        //        this.isRefreshing = false;
        //        await Application.Current.MainPage.DisplayAlert("Error", "No products", "Accept");
        //        return;
        //    }
        //    this.Products = new ObservableCollection<ProductItemViewModel>(this.ToProductItemViewModel());//Para transformar una lista en una ObservableCollection de product sólo hay que instanciar qué lista y pasarla al constructor de la observableCollection
        //    this.IsRefreshing = false;
        //}

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }

            var answer = await this.LoadProductsFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
        }



        private async Task LoadProductsFromDB()
        {
            this.list = await this.dataService.GetAllProducts();
        }

        private async void SaveProductsToDB()
        {
            await dataService.DeleteAllProducts();
           dataService.InsertAll(this.list);
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var response = await this.apiService.GetList<Product>("http://10.0.4.113", "/SalesApi/api", "/Products/", this.Category.CategoryId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.list = (List<Product>)response.Result;
            return true;
        }


        public IEnumerable<ProductItemViewModel> ToProductItemViewModel()
        {
            return this.list.Select(l => new ProductItemViewModel
            {
                Description = l.Description,
                Remarks = l.Remarks,
                Price = l.Price,
                ImagePath = l.ImagePath,
                IsAvailable = l.IsAvailable,
                ProductId = l.ProductId,
                PublishOn = l.PublishOn,

            });
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

        private void RefreshList()
        {
            if (String.IsNullOrEmpty(Filter))
            {
                var myListProductItemViewModel = this.list.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,

                });

                this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                
                    var myListProductItemViewModel = this.list.Select(p => new ProductItemViewModel
                    {
                        Description = p.Description,
                        ImageArray = p.ImageArray,
                        Price = p.Price,
                        ImagePath = p.ImagePath,
                        IsAvailable = p.IsAvailable,
                        ProductId = p.ProductId,
                        PublishOn = p.PublishOn,
                        Remarks = p.Remarks,

                    }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                    this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.Description));
                
            }
            
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        } 
        #endregion
    }
}
