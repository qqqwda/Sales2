using Sales.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Infrastructure
{
    class InstanceLocator 
    {
        public MainViewModel Main { get; set; } //Main es el objeto que todas la paginas van a bindar

        public InstanceLocator() //Instancia solo UNA instancia de la MainViewModel
        {
            this.Main = new MainViewModel();
        }
    }
}
