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
                    return Keys.IOS_ADMOB_AD_ID;
                }

                return Keys.ANDROID_ADMOB_AD_ID;
            }
        }
    }
}
