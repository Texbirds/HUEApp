using HueControllerApp.ViewModels;

namespace HueController.Views;

public partial class ConnectPage : ContentPage
{
	public ConnectPage()
	{
		InitializeComponent();
        BindingContext = new ConnectViewModel();
    }
}