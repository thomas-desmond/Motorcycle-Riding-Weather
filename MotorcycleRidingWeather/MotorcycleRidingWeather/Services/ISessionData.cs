using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;

namespace MotorcycleRidingWeather.Services
{
    public interface ISessionData
    {
        Task<ObservableCollection<DailyWeatherItem>> GetWeatherByLongLat();
    }
}