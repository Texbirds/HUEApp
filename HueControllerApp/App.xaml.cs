using HueControllerApp.Helpers;

namespace HueControllerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LocalizationHelper.SetCulture("en-US");

            MainPage = new AppShell();
        }
    }
}
