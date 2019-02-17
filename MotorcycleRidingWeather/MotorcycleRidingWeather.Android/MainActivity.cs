using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using MotorcycleRidingWeather.Constants;
using Prism;
using Prism.Ioc;

namespace MotorcycleRidingWeather.Droid
{
    [Activity(Label = "MotorcycleRidingWeather", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            MobileAds.Initialize(ApplicationContext, Keys.ADMOD_APP_ID);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            XFGloss.Droid.Library.Init(this, bundle);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

