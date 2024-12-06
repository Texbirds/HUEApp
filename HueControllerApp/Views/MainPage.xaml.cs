using HueController.ViewModels;

namespace HueController.Views
{
    [QueryProperty(nameof(IpAddress), "ip")]
    [QueryProperty(nameof(ApiKey), "key")]
    public partial class MainPage : ContentPage
    {
        public string IpAddress { get; set; }
        public string ApiKey { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (!string.IsNullOrEmpty(IpAddress) && !string.IsNullOrEmpty(ApiKey))
            {
                BindingContext = new MainViewModel(IpAddress, ApiKey);
            }
        }
    }
}
