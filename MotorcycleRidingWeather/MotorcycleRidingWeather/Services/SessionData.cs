using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DarkSkyApi;
using DarkSkyApi.Models;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Models;
using Prism.Mvvm;

namespace MotorcycleRidingWeather.Services
{
    public class SessionData : BindableBase, ISessionData
    {
        public ObservableCollection<DailyWeatherItem> SessionDailyWeatherData
        {
            get;
            set;
        }


        public async Task<ObservableCollection<DailyWeatherItem>> GetWeatherByLongLat()
        {
            var client = new DarkSkyService(Keys.DarkSkyKey);
            Forecast allForecastData = await client.GetWeatherDataAsync(33.1345692, -117.2403483);
            SessionDailyWeatherData = GrabDailyData(allForecastData);
            return SessionDailyWeatherData;
        }

        private ObservableCollection<DailyWeatherItem> GrabDailyData(Forecast allForecastData)
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
