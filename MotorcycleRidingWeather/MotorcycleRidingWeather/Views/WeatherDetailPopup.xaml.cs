using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MotorcycleRidingWeather.Views
{
    public partial class WeatherDetailPopup : PopupPage
    {
        private DailyWeatherItem dailyItem;

        public WeatherDetailPopup()
        {
            InitializeComponent();
        }

        public WeatherDetailPopup(DailyWeatherItem dailyItem)
        {
            InitializeComponent();
            this.dailyItem = dailyItem;
            BindingContext = this.dailyItem;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return Content.FadeTo(1);
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return Content.FadeTo(1);
        }
    }
}
