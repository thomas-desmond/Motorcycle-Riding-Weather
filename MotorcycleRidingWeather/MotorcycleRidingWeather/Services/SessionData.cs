using System;
using System.Threading.Tasks;
using DarkSkyApi;
using DarkSkyApi.Models;
using MotorcycleRidingWeather.Constants;
using Prism.Mvvm;

namespace MotorcycleRidingWeather.Services
{
    public class SessionData : BindableBase, ISessionData
    {
        public SessionData()
        {
        }



        public async Task<Forecast> GetWeatherByLongLat()
        {
            var client = new DarkSkyService(Keys.DarkSkyKey);
            Forecast result = await client.GetWeatherDataAsync(33.1345692, -117.2403483);
            return result;
        }
    }
}
