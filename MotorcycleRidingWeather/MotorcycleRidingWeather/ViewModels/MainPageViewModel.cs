using Prism.Navigation;
using MotorcycleRidingWeather.Models;
using System.Collections.ObjectModel;
using Prism.Commands;
using MotorcycleRidingWeather.Constants;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using MotorcycleRidingWeather.Services;
using Prism.Services;

namespace MotorcycleRidingWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToSettingsPage { get; set; }

        INavigationService _navigationService;
        ISessionData _sessionData;
        IPageDialogService _pageDialogService;

        private ObservableCollection<DailyWeatherItem> _weatherDisplayInformation;
        public ObservableCollection<DailyWeatherItem> WeatherDisplayInformation
        {
            get { return _weatherDisplayInformation; }
            set { SetProperty(ref _weatherDisplayInformation, value); }
        }

        private DailyWeatherItem _todayWeather;
        public DailyWeatherItem TodayWeather
        {
            get { return _todayWeather; }
            set { SetProperty(ref _todayWeather, value); }
        }

        public string AddUnitId
        {
            get
            {
                return Keys.ADMOB_AD_ID;
            }
        }

        private bool _isRefreshActive;
        public bool IsRefreshActive
        {
            get { return _isRefreshActive; }
            set { SetProperty(ref _isRefreshActive, value); }
        }

        public MainPageViewModel(INavigationService navigationService,
                                 ISessionData sessionData,
                                IPageDialogService pageDialogService)
            : base(navigationService)
        {
            NavigateToSettingsPage = new DelegateCommand(OnNavigateToSettingsPage);

            _sessionData = sessionData;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
        }

        private async void OnNavigateToSettingsPage()
        {
            await _navigationService.NavigateAsync(PageNames.SettingsPageName);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            var zipCode = SessionData.CurrentUserPreferences.LocationZipCode;
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                zipCode = "Set A Location";
            }
            Title = zipCode;
            IsRefreshActive = true;
            var weatherInfo = await _sessionData.GetWeatherByZipCode(zipCode);
            WeatherDisplayInformation =
                new ObservableCollection<DailyWeatherItem>(weatherInfo);
            IsRefreshActive = false;
            Settings.UserChangedLocation = false;
            if (_sessionData.SessionDailyWeatherData.Count > 0)
            {
                TodayWeather = _sessionData.SessionDailyWeatherData[0];
                WeatherDisplayInformation.RemoveAt(0);
            }
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrWhiteSpace(SessionData.CurrentUserPreferences.LocationZipCode))
            {
                await _pageDialogService.DisplayAlertAsync("Set A Location",
                                                          "It looks like this if your first time using the app or you do not have a location set, let's do that now",
                                                           "OK");
                await _navigationService.NavigateAsync(PageNames.SettingsPageName);
            }
        }
    }
}
