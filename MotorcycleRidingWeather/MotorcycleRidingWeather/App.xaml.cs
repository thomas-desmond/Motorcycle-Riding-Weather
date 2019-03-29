using Prism;
using Prism.Ioc;
using MotorcycleRidingWeather.ViewModels;
using MotorcycleRidingWeather.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MotorcycleRidingWeather.Services;
using Prism.DryIoc;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MotorcycleRidingWeather
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();

            containerRegistry.Register<ISessionData, SessionData>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            AppCenter.Start("android=bd9c6543-5e1d-424d-975f-25c679571f39;" + "ios=875d9205-a2be-49a8-8c3e-08cada6b0588;", typeof(Analytics), typeof(Crashes));
        }

    }
}
