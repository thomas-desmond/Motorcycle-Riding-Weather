using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MotorcycleRidingWeather.Models;

namespace MotorcycleRidingWeather.Services
{
    public interface ISessionData
    {
        ObservableCollection<DailyWeatherItem> SessionDailyWeatherData { get; set; }
        Task<ObservableCollection<DailyWeatherItem>> GetWeatherByZipCode(string zipCode);
        Task<ObservableCollection<DailyWeatherItem>> GetWeatherBySettingValue();
    }
}