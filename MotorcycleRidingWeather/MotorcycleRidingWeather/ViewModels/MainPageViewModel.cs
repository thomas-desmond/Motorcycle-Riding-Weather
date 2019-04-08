using Prism.Navigation;
using MotorcycleRidingWeather.Models;
using System.Collections.ObjectModel;
using Prism.Commands;
using MotorcycleRidingWeather.Constants;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using MotorcycleRidingWeather.Services;
using Prism.Services;
using System;
using MotorcycleRidingWeather.Views;
using Rg.Plugins.Popup.Services;

namespace MotorcycleRidingWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand NavigateToSettingsPage { get; set; }
        public DelegateCommand OpenWeatherDetailPopup { get; set; }


        INavigationService _navigationService;
        ISessionData _sessionData;
        IPageDialogService _pageDialogService;

        private WeatherDetailPopup _weatherDetailPopup;


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

        private bool _shouldShowPage = false;
        public bool ShouldShowPage
        {
            get { return _shouldShowPage; }
            set
            {
                SetProperty(ref _shouldShowPage, value);
                RaisePropertyChanged(nameof(ShowActivityIndicator));
            }
        }

        public bool ShowActivityIndicator
        {
            get
            {
                return !ShouldShowPage;
            }
        }

        public string AddUnitId
        {
            get
            {
                return Keys.ANDROID_ADMOB_AD_ID;
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
            OpenWeatherDetailPopup = new DelegateCommand(OnOpenWeatherDetailPopup);

            _sessionData = sessionData;
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            _weatherDetailPopup = new WeatherDetailPopup();
        }

        private async void OnOpenWeatherDetailPopup()
        {
            await PopupNavigation.Instance.PushAsync(_weatherDetailPopup);
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
            else
            {
                Title = zipCode;
                IsRefreshActive = true;
                var weatherInfo = await _sessionData.GetWeatherByZipCode(zipCode);
                if (weatherInfo.Count == 0)
                {
                    await _pageDialogService.DisplayAlertAsync("Error", "I was unable to retrieve weather data, please check your zip code is correct", "OK");
                    IsRefreshActive = false;
                    ShouldShowPage = true;
                    return;
                }
                WeatherDisplayInformation =
                    new ObservableCollection<DailyWeatherItem>(weatherInfo);
                IsRefreshActive = false;
                if (_sessionData.SessionDailyWeatherData.Count > 0)
                {
                    TodayWeather = _sessionData.SessionDailyWeatherData[0];
                    WeatherDisplayInformation.RemoveAt(0);
                }
                ShouldShowPage = true;
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
