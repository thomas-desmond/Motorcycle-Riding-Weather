using System;
using System.Linq;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Models;
using MotorcycleRidingWeather.Services;
using Plugin.Messaging;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace MotorcycleRidingWeather.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        readonly INavigationService _navigationService;
        readonly ISessionData _sessionData;
        readonly IPageDialogService _pageDialogService;

        public DelegateCommand NavigateBack { get; set; }
        public DelegateCommand GetRidingWeatherCommand { get; set; }
        public DelegateCommand SendFeedbackCommand { get; set; }

        private UserPreferences _userPreferences;
        public UserPreferences UserPreferences
        {
            get { return _userPreferences; }
            set { SetProperty(ref _userPreferences, value); }
        }

        public SettingsPageViewModel(INavigationService navigationService,
                                     ISessionData sessionData,
                                    IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Settings";

            NavigateBack = new DelegateCommand(OnNavigateBack);
            GetRidingWeatherCommand = new DelegateCommand(OnGetRidingWeather);
            SendFeedbackCommand = new DelegateCommand(SendFeedback);


            _navigationService = navigationService;
            _sessionData = sessionData;
            _pageDialogService = pageDialogService;
        }

        void SendFeedback()
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;

            if (emailMessenger.CanSendEmail == false)
            {
                return;
            }
            emailMessenger.SendEmail("motorcycleridingweather@gmail.com",
                                "Motorcycle Riding Weather Feedback",
                                "Please provide any feedback below on the app " +
                                 "such as issues, feature requests, or anything else\n\n" +
                                 "Thanks!");
        }
        async void OnGetRidingWeather()
        {
            if (UserPreferences.LocationZipCode.All(char.IsDigit) == false
                || UserPreferences.LocationZipCode.Length > 5)
            {
                await _pageDialogService.DisplayAlertAsync("Invalid Zip Code", "Currently you can only search locations by zip code, please ensure zip code is entered correctly. Ability to search by city coming soon", "Close");
            }
            Settings.UserChangedLocation = true;
        }

        void OnNavigateBack()
        {
            _sessionData.SaveUserData(UserPreferences);
            _navigationService.GoBackAsync();
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            InitalizePageWithUserSettings();
        }

        void InitalizePageWithUserSettings()
        {
            UserPreferences = _sessionData.GetCurrentUserPreferences();
        }
    }
}