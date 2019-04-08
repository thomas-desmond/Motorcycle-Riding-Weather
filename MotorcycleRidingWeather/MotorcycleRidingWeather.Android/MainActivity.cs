using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using MotorcycleRidingWeather.Constants;
using MotorcycleRidingWeather.Views;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup.Services;

namespace MotorcycleRidingWeather.Droid
{
    [Activity(Label = "MotorcycleRidingWeather", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);
            MobileAds.Initialize(ApplicationContext, Keys.ADMOD_APP_ID);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
            XFGloss.Droid.Library.Init(this, bundle);
        }

        public override async void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                base.OnBackPressed();
            }
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

