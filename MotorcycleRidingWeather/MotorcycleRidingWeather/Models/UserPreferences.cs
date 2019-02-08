using System;
namespace MotorcycleRidingWeather.Models
{
    public class UserPreferences
    {
        public string LocationZipCode
        {
            get;
            set;
        }

        public int MaxRidingTemp
        {
            get;
            set;
        }

        public int MinRidingTemp
        {
            get;
            set;
        }

        public int MaxRainPercentage
        {
            get;
            set;
        }
    }
}
