using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HueController.Domain.Models;
using HueController.Infrastructure.ApiClients;
using Microsoft.Maui.Controls;

namespace HueControllerApp.ViewModels
{
    public class ConnectViewModel : BaseViewModel
    {
        private readonly HueBridgeApiClient _apiClient;

        public string BridgeIp { get; set; }
        public string ConnectionStatus { get; set; }
        public ObservableCollection<Bridge> ConnectedBridges { get; set; }
        public ICommand ConnectCommand { get; }
        public ICommand OpenBridgeCommand { get; }

        public ConnectViewModel()
        {
            _apiClient = new HueBridgeApiClient();
            ConnectedBridges = new ObservableCollection<Bridge>();

            ConnectCommand = new Command(async () => await ConnectToBridge());
            OpenBridgeCommand = new Command<Bridge>(async (bridge) => await OpenBridgeScreen(bridge));
        }

        private async Task ConnectToBridge()
        {
            try
            {
                ConnectionStatus = "Connecting...";
                OnPropertyChanged(nameof(ConnectionStatus));

                var apiKey = await _apiClient.RegisterAppAsync(BridgeIp, "my_hue_app");
                ConnectionStatus = "Connected! API Key: " + apiKey;

                var bridge = new Bridge { IpAddress = BridgeIp, ApiKey = apiKey };
                ConnectedBridges.Add(bridge);

                BridgeIp = string.Empty;
                OnPropertyChanged(nameof(BridgeIp));
            }
            catch (Exception ex)
            {
                ConnectionStatus = "Error: " + ex.Message;
            }

            OnPropertyChanged(nameof(ConnectionStatus));
        }

        private async Task OpenBridgeScreen(Bridge bridge)
        {
            if (bridge == null) return;

            await Shell.Current.GoToAsync($"///MainPage?ip={bridge.IpAddress}&key={bridge.ApiKey}");
        }
    }
}
