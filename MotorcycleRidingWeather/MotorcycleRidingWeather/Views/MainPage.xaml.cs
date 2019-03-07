using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MotorcycleRidingWeather.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemclicked = (DailyWeatherItem)button.CommandParameter;
            await PopupNavigation.Instance.PushAsync(new WeatherDetailPopup(itemclicked));
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var button = (ListView)sender;
            var itemclicked = (DailyWeatherItem)button.SelectedItem;
            await PopupNavigation.Instance.PushAsync(new WeatherDetailPopup(itemclicked));
        }
    }
}