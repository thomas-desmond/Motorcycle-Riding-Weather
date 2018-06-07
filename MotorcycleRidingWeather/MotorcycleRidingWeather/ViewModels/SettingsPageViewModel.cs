using System;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Services;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Commands;
using Prism.Navigation;

namespace MotorcycleRidingWeather.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        INavigationService _navigationService;
        ISessionData _sessionData;

        public DelegateCommand NavigateBack { get; set; }
        public DelegateCommand GetRidingWeatherCommand { get; set; }

        private string _temperatureScaleType;
        public string TemperatureScaleType
        {
            get { return _temperatureScaleType; }
            set { SetProperty(ref _temperatureScaleType, value); }
        }

        private string _maxRidingTempLabel = "Max riding temp (F)";
        public string MaxRidingTempLabel
        {
            get { return _maxRidingTempLabel; }
            set { SetProperty(ref _maxRidingTempLabel, value); }
        }

        private string _minRidingTempLabel = "Min riding tmep (F)";
        public string MinRidingTempLabel
        {
            get { return _minRidingTempLabel; }
            set { SetProperty(ref _minRidingTempLabel, value); }
        }

        private string _locationText;
        public string LocationText
        {
            get { return _locationText; }
            set { SetProperty(ref _locationText, value); }
        }

        private bool _isFahrenheit;
        public bool IsFahrenheit
        {
            get { return _isFahrenheit; }
            set
            {
                SetProperty(ref _isFahrenheit, value);
                if (IsFahrenheit)
                {
                    TemperatureScaleType = "Fahrenheit";
                    MaxRidingTempLabel = "Max riding temp (F)";
                    MinRidingTempLabel = "Min riding tmep (F)";
                }
                else
                {
                    TemperatureScaleType = "Celcius";
                    MaxRidingTempLabel = "Max riding temp (C)";
                    MinRidingTempLabel = "Min riding tmep (C)";
                }
            }
        }

        private int _minRidingTemp;
        public int MinRidingTemp
        {
            get { return _minRidingTemp; }
            set { SetProperty(ref _minRidingTemp, value); }
        }

        private int _maxRidingTemp;
        public int MaxRidingTemp
        {
            get { return _maxRidingTemp; }
            set { SetProperty(ref _maxRidingTemp, value); }
        }


        public SettingsPageViewModel(INavigationService navigationService,
                                     ISessionData sessionData)
            : base(navigationService)
        {
            Title = "Settings";

            NavigateBack = new DelegateCommand(OnNavigateBack);
            GetRidingWeatherCommand = new DelegateCommand(OnGetRidingWeather);

            _navigationService = navigationService;
            _sessionData = sessionData;
        }

        private void OnGetRidingWeather()
        {
            Settings.UserChangedLocation = true;
        }

        private void OnNavigateBack()
        {
            SaveUserSettings();
            _navigationService.GoBackAsync();
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            InitalizePageWithUserSettings();
        }

        private void InitalizePageWithUserSettings()
        {
            MaxRidingTemp = AppSettings.GetValueOrDefault(AppSettingKeys.USER_MAX_TEMP, 90);
            MinRidingTemp = AppSettings.GetValueOrDefault(AppSettingKeys.USER_MIN_TEMP, 40);
            LocationText = AppSettings.GetValueOrDefault(AppSettingKeys.USER_LOCATION, string.Empty);
        }

        private async void SaveUserSettings()
        {
            AppSettings.AddOrUpdateValue(AppSettingKeys.USER_MAX_TEMP, MaxRidingTemp);
            AppSettings.AddOrUpdateValue(AppSettingKeys.USER_MIN_TEMP, MinRidingTemp);
            AppSettings.AddOrUpdateValue(AppSettingKeys.USER_LOCATION, LocationText);
        }
    }
}