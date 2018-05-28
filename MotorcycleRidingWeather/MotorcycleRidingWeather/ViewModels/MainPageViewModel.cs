using Prism.Navigation;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;
using System.Collections.ObjectModel;
using Prism.Commands;
using MotorcycleRidingWeather.Constants;

namespace MotorcycleRidingWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToSettingsPage { get; set; }

        INavigationService _navigationService;

        private ObservableCollection<DataList> _weatherCollection;
        public ObservableCollection<DataList> WeatherCollection
        {
            get { return _weatherCollection; }
            set { SetProperty(ref _weatherCollection, value); }
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
            var fiveDayAllData = await GetWeatherByZipCode();
            WeatherCollection = new ObservableCollection<DataList>(fiveDayAllData.DataList);
        }

		internal async Task<FiveDayWeatherItem> GetWeatherByZipCode()
        {
            var client = new HttpClient();
            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri =
                new Uri("http://api.openweathermap.org/data/2.5/forecast?zip=92027,us&units=imperial&appid=4bd567dca0d90153a821781e2b6a9574");
            HttpResponseMessage response = await client.SendAsync(httpRequest);
            FiveDayWeatherItem fiveDayWeatherItemCollection = null;
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                fiveDayWeatherItemCollection = FiveDayWeatherItem.FromJson(jsonContent);
            }
            return fiveDayWeatherItemCollection;
        }
    }
}
