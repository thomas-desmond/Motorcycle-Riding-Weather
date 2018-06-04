using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Navigation;

namespace MotorcycleRidingWeather.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private static ISettings AppSettings => CrossSettings.Current;
        INavigationService _navigationService;

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
                    //ConvertTempsToFahrenheit();
                }
                else
                {
                    TemperatureScaleType = "Celcius";
                    MaxRidingTempLabel = "Max riding temp (C)";
                    MinRidingTempLabel = "Min riding tmep (C)";
                    //ConvertTmpsToCelcius();
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

        public SettingsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Settings";

            _navigationService = navigationService;
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            InitalizePageWithUserSettings();
        }

        private void InitalizePageWithUserSettings()
        {
            MaxRidingTemp = AppSettings.GetValueOrDefault(nameof(MaxRidingTemp), 90);
            MinRidingTemp = AppSettings.GetValueOrDefault(nameof(MinRidingTemp), 40);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            SaveUserSettings();
        }

        private void SaveUserSettings()
        {
            AppSettings.AddOrUpdateValue(nameof(MaxRidingTemp), MaxRidingTemp);
            AppSettings.AddOrUpdateValue(nameof(MinRidingTemp), MinRidingTemp);
        }

        private void ConvertTempsToFahrenheit()
        {
            MinRidingTemp = (int)(((9.0 / 5.0) * MinRidingTemp) + 32);
            MaxRidingTemp = (int)((9.0 / 5.0) * MaxRidingTemp) + 32;

        }

        private void ConvertTmpsToCelcius()
        {
            MinRidingTemp = (int)((5.0 / 9.0) * (MinRidingTemp - 32));
            MaxRidingTemp = (int)((5.0 / 9.0) * (MaxRidingTemp - 32));
        }
    }
}
