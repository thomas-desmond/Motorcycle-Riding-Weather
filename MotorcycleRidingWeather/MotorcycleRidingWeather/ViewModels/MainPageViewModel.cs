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

namespace MotorcycleRidingWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToSettingsPage { get; set; }

        INavigationService _navigationService;

        private ObservableCollection<DailyWeatherItem> _weatherDisplayInformation;
        public ObservableCollection<DailyWeatherItem> WeatherDisplayInformation
        {
            get { return _weatherDisplayInformation; }
            set { SetProperty(ref _weatherDisplayInformation, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            NavigateToSettingsPage = new DelegateCommand(OnNavigateToSettingsPage);

            _navigationService = navigationService;
        }

		private async void OnNavigateToSettingsPage()
        {
            await _navigationService.NavigateAsync(PageNames.SettingsPageName);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
		{
            var allForecastData = await GetWeatherByLongLat();
            var allDailyDataToDisplay = GrabDailyDataNeeded(allForecastData);
            WeatherDisplayInformation = allDailyDataToDisplay;
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

        internal async Task<Forecast> GetWeatherByLongLat()
        {
            var client = new DarkSkyService(Keys.DarkSkyKey);
            Forecast result = await client.GetWeatherDataAsync(33.1345692, -117.2403483);
            return result;

        }
    }
}
