using Prism.Navigation;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;
using System.Collections.ObjectModel;
using Prism.Commands;
using MotorcycleRidingWeather.Constants;
using DarkSkyApi;
using DarkSkyApi.Models;
using Xamarin.Forms;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using MotorcycleRidingWeather.Services;

namespace MotorcycleRidingWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public DelegateCommand NavigateToSettingsPage { get; set; }

        INavigationService _navigationService;
        ISessionData _sessionData;

        private ObservableCollection<DailyWeatherItem> _weatherDisplayInformation;
        public ObservableCollection<DailyWeatherItem> WeatherDisplayInformation
        {
            get { return _weatherDisplayInformation; }
            set { SetProperty(ref _weatherDisplayInformation, value); }
        }

        public MainPageViewModel(INavigationService navigationService,
                                 ISessionData sessionData)
            : base(navigationService)
        {
            _sessionData = sessionData;

            Title = "San Marcos";
            NavigateToSettingsPage = new DelegateCommand(OnNavigateToSettingsPage);

            _navigationService = navigationService;
        }

		private async void OnNavigateToSettingsPage()
        {
            await _navigationService.NavigateAsync(PageNames.SettingsPageName);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
		{
            if (WeatherDisplayInformation == null
                || WeatherDisplayInformation.Count <= 0)
            {
                var allForecastData = await _sessionData.GetWeatherByLongLat();
                var allDailyDataToDisplay = GrabDailyDataNeeded(allForecastData);
                WeatherDisplayInformation = allDailyDataToDisplay;
            }
            else
            {
                WeatherDisplayInformation = new ObservableCollection<DailyWeatherItem>(WeatherDisplayInformation);
            }
        }

        private ObservableCollection<DailyWeatherItem> GrabDailyDataNeeded(Forecast allForecastData)
        {
            var dailyInfoToDisplay = new ObservableCollection<DailyWeatherItem>();
            foreach (var day in allForecastData.Daily.Days)
            {
                var dayInfo = new DailyWeatherItem()
                {
                    HighTemperature = day.HighTemperature,
                    LowTemperature = day.LowTemperature,
                    WindSpeed = day.WindSpeed,
                    PrecipitationProbability = day.PrecipitationProbability,
                    Time = day.Time,
                };

                dailyInfoToDisplay.Add(dayInfo);
            }
            return dailyInfoToDisplay;
        }
    }
}
