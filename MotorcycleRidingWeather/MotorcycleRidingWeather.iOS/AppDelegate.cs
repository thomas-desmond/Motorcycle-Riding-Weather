using Foundation;
//using Google.MobileAds;
using Prism;
using Prism.Ioc;
using UIKit;


namespace MotorcycleRidingWeather.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            XFGloss.iOS.Library.Init();
            Rg.Plugins.Popup.Popup.Init();
            //MobileAds.Configure("ca-app-pub-6670943348936598~8658832182");

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new iOSInitializer()));
            //Firebase.Core.App.Configure();

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {

        }
    }
}
