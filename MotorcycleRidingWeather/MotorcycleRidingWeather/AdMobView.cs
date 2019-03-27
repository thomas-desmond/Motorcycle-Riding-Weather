using System;
using MotorcycleRidingWeather.Constants;
using Xamarin.Forms;

namespace MotorcycleRidingWeather
{
    public class AdMobView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                   nameof(AdUnitId),
                   typeof(string),
                   typeof(AdMobView),
                   string.Empty);

        public string AdUnitId
        {
            get
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    return "ca-app-pub-3940256099942544/2934735716";
                }

                return Keys.ANDROID_ADMOB_AD_ID;
            }
        }
    }
}
