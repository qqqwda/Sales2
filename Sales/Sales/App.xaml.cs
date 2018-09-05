using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Sales
{
    using Views;
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new ProductsPage());//
            //MainPage = new ProductsPage(); //Arranca por ProductsPage
			//MainPage = new MainPage(); //crea una MainPage.xaml de nombre MainPage y la inicializa, haci�ndola la p�gina principal
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
