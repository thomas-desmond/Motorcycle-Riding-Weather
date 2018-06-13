using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DarkSkyApi;
using DarkSkyApi.Models;
using Geocoding;
using Geocoding.MapQuest;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Models;
using Prism.Mvvm;

namespace MotorcycleRidingWeather.Services
{
    public class SessionData : BindableBase, ISessionData
    {
        internal IGeocoder geocoder;
        internal DarkSkyService darkSkyService;

        public SessionData()
        {
            geocoder = new MapQuestGeocoder(Keys.MapQuestKey);
            darkSkyService = new DarkSkyService(Keys.DarkSkyKey);

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
                };

                dailyInfoToDisplay.Add(dayInfo);
            }
            return dailyInfoToDisplay;
        }
    }
}
