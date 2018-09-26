using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sales.Helpers
{
    public static class ChangeImageHelper
    {
        private static async void ChangeImage(MediaFile file, ImageSource imageSource)
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
                file = null;
                return;
            }
            if (source.Equals("New Picture"))
            {
                file = await CrossMedia.Current.TakePhotoAsync(
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
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                imageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
    }
}
