using System;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Models;
using MotorcycleRidingWeather.Services;
using Plugin.Messaging;
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
        public DelegateCommand SendFeedbackCommand { get; set; }

        private UserPreferences _userPreferences;
        public UserPreferences UserPreferences
        {
            get { return _userPreferences; }
            set { SetProperty(ref _userPreferences, value); }
        }

        public SettingsPageViewModel(INavigationService navigationService,
                                     ISessionData sessionData)
            : base(navigationService)
        {
            Title = "Settings";

            NavigateBack = new DelegateCommand(OnNavigateBack);
            GetRidingWeatherCommand = new DelegateCommand(OnGetRidingWeather);
            SendFeedbackCommand = new DelegateCommand(SendFeedback);


            _navigationService = navigationService;
            _sessionData = sessionData;
        }

        private void SendFeedback()
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
        private void OnGetRidingWeather()
        {
            Settings.UserChangedLocation = true;
        }

        private void OnNavigateBack()
        {
            _sessionData.SaveUserData(UserPreferences);
            _navigationService.GoBackAsync();
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            InitalizePageWithUserSettings();
        }

        private void InitalizePageWithUserSettings()
        {
            UserPreferences = _sessionData.GetCurrentUserPreferences();
        }
    }
}