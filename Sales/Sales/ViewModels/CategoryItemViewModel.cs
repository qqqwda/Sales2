using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sales.ViewModels
{
    public class CategoryItemViewModel : Category
    {
        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GoToCategory);
            }
        }

        private async void GoToCategory()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel(this); //this);
            await App.Navigator.PushAsync(new ProductsPage());
        }
    }
}
