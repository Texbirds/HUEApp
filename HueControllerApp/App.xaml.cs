using HueControllerApp.Helpers;

namespace HueControllerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LocalizationHelper.SetCulture("nl-NL");

            MainPage = new AppShell();
        }
    }
}
