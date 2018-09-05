using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
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
    public class ProductsViewModel : BaseViewModel
    {
        #region Prop
        private bool isRefreshing;
        private ApiService apiService;
        private ObservableCollection<ProductItemViewModel> products;
        private List<Product> list;

        #endregion


        #region Singleton 
        //se utiliza para no volver a instanciar la ProductsViewModel
        //cada vez que se llama, y retorna la ProductsViewModel que está
        //en memoria
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if(instance == null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }
        #endregion

        #region Atributos
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        } //atributo observableCollection de tipo Product(Common) llamada "Products" 
        #endregion

        #region Constructor

        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        } 
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.isRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                return;
            }

            var response = await this.apiService.GetList<Product>("http://10.0.4.113", "/SalesApi/api", "/Products");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            this.list = (List<Product>)response.Result;//El servicio no nos retorna ObservableCollections, nos retorna una Lista. Transformamos el result del response.Result en una Lista del modelo Product
            this.Products = new ObservableCollection<ProductItemViewModel>(this.ToProductItemViewModel());//Para transformar una lista en una ObservableCollection de product sólo hay que instanciar qué lista y pasarla al constructor de la observableCollection
            
            this.IsRefreshing = false;
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
