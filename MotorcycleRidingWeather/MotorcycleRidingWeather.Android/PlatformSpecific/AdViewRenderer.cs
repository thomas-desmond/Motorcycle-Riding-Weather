using Android.Widget;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using MotorcycleRidingWeather.Constants;

[assembly: ExportRenderer(typeof(MotorcycleRidingWeather.Controls.AdControlView), typeof(MotorcycleRidingWeather.Droid.PlatformSpecific.AdViewRenderer))]
namespace MotorcycleRidingWeather.Droid.PlatformSpecific
{
    public class AdViewRenderer : ViewRenderer<Controls.AdControlView, AdView>
    {
        public AdViewRenderer(Context context) : base(context) { }

        string adUnitId = string.Empty;
        //Note you may want to adjust this, see further down.
        AdSize adSize = AdSize.SmartBanner;
        AdView adView;
        AdView CreateNativeAdControl()
        {
            if (adView != null)
                return adView;

            // This is a string in the Resources/values/strings.xml that I added or you can modify it here. This comes from admob and contains a / in it
            adUnitId = Keys.ANDROID_ADMOB_AD_ID;
            adView = new AdView(Forms.Context);
            adView.AdSize = adSize;
            adView.AdUnitId = adUnitId;

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            adView.LayoutParameters = adParams;

            adView.LoadAd(new AdRequest
                            .Builder()
                            .Build());
            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.AdControlView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                CreateNativeAdControl();
                SetNativeControl(adView);
            }
        }
    }
}
