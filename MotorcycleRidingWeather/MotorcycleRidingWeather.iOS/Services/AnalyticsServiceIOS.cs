using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;
using MotorcycleRidingWeather.iOS.Services;
using MotorcycleRidingWeather.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AnalyticsServiceIOS))]
namespace MotorcycleRidingWeather.iOS.Services
{
    public class AnalyticsServiceIOS : IAnalyticsService
    {

        public void LogEvent(string eventId)
        {
            LogEvent(eventId, (IDictionary<string, string>)null);
        }

        public void LogEvent(string eventId, string paramName, string value)
        {
            LogEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void LogEvent(string eventId, IDictionary<string, string> parameters)
        {
            
            if (parameters == null)
            {
                var nullDict = new NSDictionary<NSString, NSObject>();
                nullDict = null;
                Analytics.LogEvent(eventId, nullDict);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary =
                NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);

        }
    }
}
