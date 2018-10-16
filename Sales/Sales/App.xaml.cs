using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Sales
{
    using Newtonsoft.Json;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.ViewModels;
    using Views;
	public partial class App : Application
	{
        public static NavigationPage Navigator { get; internal set; }

        public App ()
		{
			InitializeComponent();

            var mainViewModel = MainViewModel.GetInstance();

            if (Settings.IsRemembered)
            {
                if (string.IsNullOrEmpty(Settings.UserASP))
                {
                    mainViewModel.UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);
                }
                mainViewModel.Products = new ProductsViewModel();
                this.MainPage = new MasterPage();
            }
            else
            {
                mainViewModel.Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }

            //if (Settings.IsRemembered && !string.IsNullOrEmpty(Settings.AccessToken))
            //{
            //    MainViewModel.GetInstance().Products = new ProductsViewModel();
            //    MainPage = new MasterPage(); //Contiene navigationPage
            //}
            //else
            //{
            //    MainViewModel.GetInstance().Login = new LoginViewModel();
            //    MainPage = new NavigationPage(new LoginPage());
            //    // MainPage = new NavigationPage(new ProductsPage());//
            //    //MainPage = new ProductsPage(); //Arranca por ProductsPage
            //    //MainPage = new MainPage(); //crea una MainPage.xaml de nombre MainPage y la inicializa, haciéndola la página principal
            //}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
