﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DarkSkyApi;
using DarkSkyApi.Models;
using Geocoding;
using Geocoding.MapQuest;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;

namespace MotorcycleRidingWeather.Services
{
    public class SessionData : BindableBase, ISessionData
    {
        internal IGeocoder geocoder;
        internal DarkSkyService darkSkyService;
        private static ISettings SavedUserSettings => CrossSettings.Current;


        public SessionData()
        {
            geocoder = new MapQuestGeocoder(Keys.MapQuestKey);
            darkSkyService = new DarkSkyService(Keys.DarkSkyKey);
            CurrentUserPreferences = new UserPreferences();
            LoadCurrentUserPreferences();
        }

        public static UserPreferences CurrentUserPreferences
        {
            get;
            set;
        }

        private ObservableCollection<DailyWeatherItem> _sessionDailyWeatherData;
        public ObservableCollection<DailyWeatherItem> SessionDailyWeatherData
        {
            get { return _sessionDailyWeatherData; }
            set { SetProperty(ref _sessionDailyWeatherData, value); }
        }

        public async Task<ObservableCollection<DailyWeatherItem>> GetWeatherByZipCode(string zipCode)
        {
            var locationData = await geocoder.GeocodeAsync(zipCode);
            var latitude = locationData.First().Coordinates.Latitude;
            var longitude = locationData.First().Coordinates.Longitude;
            Forecast allForecastData = await darkSkyService.GetWeatherDataAsync(latitude, longitude);
            SessionDailyWeatherData = GrabDailyData(allForecastData);
            return SessionDailyWeatherData;
        }


        public async Task<ObservableCollection<DailyWeatherItem>> GetWeatherBySettingValue()
        {
            Forecast allForecastData = await darkSkyService.GetWeatherDataAsync(33.1345692, -117.2403483);
            SessionDailyWeatherData = GrabDailyData(allForecastData);
            return SessionDailyWeatherData;
        }

        private ObservableCollection<DailyWeatherItem> GrabDailyData(Forecast allForecastData)
        {
            var dailyInfoToDisplay = new ObservableCollection<DailyWeatherItem>();

            DateTimeOffset? currentDay = null;
            var dailyInfoExcludingIgnoreTimes = new DailyWeatherItem();
            double rainAccumlationHour = 0;
            foreach (var hour in allForecastData.Hourly.Hours)
            {

                if (currentDay == null || hour.Time.Day != currentDay.Value.Day)
                {
                    currentDay = hour.Time;
                }

            }

            foreach (var day in allForecastData.Daily.Days)
            {
                var dayInfo = new DailyWeatherItem()
                {
                    HighTemperature = day.HighTemperature,
                    LowTemperature = day.LowTemperature,
                    WindSpeed = day.WindSpeed,
                    PrecipitationProbability = day.PrecipitationProbability,
                    Time = day.Time,
                    PrecipitationType = day.PrecipitationType,
                    CloudCover = day.CloudCover,
                    UVIndex = day.UVIndex,
                    WindGust = day.WindGust,
                    Icon = day.Icon,
                    RainAccummulationCalculatedByDaily = day.PrecipitationIntensity * 24,
                    RainAccummulationCalculatedByHourly = GetHourlyRain(allForecastData, day.Time.Day),
                };


                if (dayInfo.RainAccummulationCalculatedByHourly < .01)
                {
                    dayInfo.PrecipitationProbability = 0;
                    rainAccumlationHour = 0;
                }
                dailyInfoToDisplay.Add(dayInfo);
            }
            return dailyInfoToDisplay;
        }

        private double GetHourlyRain(Forecast allForecastData, int day)
        {
            double rainAccumulation = 0;
            foreach (var hour in allForecastData.Hourly.Hours)
            {
                if (hour.Time.Day == day)
                {
                    rainAccumulation += hour.PrecipitationIntensity;
                }
            }
            return rainAccumulation;
        }

        public UserPreferences GetCurrentUserPreferences()
        {
            return CurrentUserPreferences;
        }

        public UserPreferences LoadCurrentUserPreferences()
        {
            CurrentUserPreferences.MaxRidingTemp = SavedUserSettings.GetValueOrDefault(AppSettingKeys.USER_MAX_TEMP, 90);
            CurrentUserPreferences.MinRidingTemp = SavedUserSettings.GetValueOrDefault(AppSettingKeys.USER_MIN_TEMP, 40);
            CurrentUserPreferences.LocationZipCode = SavedUserSettings.GetValueOrDefault(AppSettingKeys.USER_LOCATION, string.Empty);
            CurrentUserPreferences.MaxRainPercentage = SavedUserSettings.GetValueOrDefault(AppSettingKeys.USER_MAX_PRECIP_PERCENT, 5);
            CurrentUserPreferences.MaxRainAccumulation = SavedUserSettings.GetValueOrDefault(AppSettingKeys.USER_MAX_RAIN_ACCUMULATION, 1.0);

            return CurrentUserPreferences;
        }

        public void SaveUserData(UserPreferences newUserPreferces)
        {
            CurrentUserPreferences = newUserPreferces;
            SavedUserSettings.AddOrUpdateValue(AppSettingKeys.USER_MAX_TEMP, newUserPreferces.MaxRidingTemp);
            SavedUserSettings.AddOrUpdateValue(AppSettingKeys.USER_MIN_TEMP, newUserPreferces.MinRidingTemp);
            SavedUserSettings.AddOrUpdateValue(AppSettingKeys.USER_LOCATION, newUserPreferces.LocationZipCode);
            SavedUserSettings.AddOrUpdateValue(AppSettingKeys.USER_MAX_PRECIP_PERCENT, newUserPreferces.MaxRainPercentage);
            SavedUserSettings.AddOrUpdateValue(AppSettingKeys.USER_MAX_RAIN_ACCUMULATION, newUserPreferces.MaxRainAccumulation);

        }

    }
}
