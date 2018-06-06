using System.Threading.Tasks;
using DarkSkyApi.Models;

namespace MotorcycleRidingWeather.Services
{
    public interface ISessionData
    {
        Task<Forecast> GetWeatherByLongLat();
    }
}