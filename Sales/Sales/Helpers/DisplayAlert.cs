using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sales.Helpers
{
    public static class DisplayAlert
    {
        public static async void Error(string description, string button)
        {
            await Application.Current.MainPage.DisplayAlert("Error", description, button);
        }

        public static async void Alert(string description, string button)
        {
            await Application.Current.MainPage.DisplayAlert("Warning", description, button);
        }
    }
}
