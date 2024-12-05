using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HueController.Infrastructure.ApiClients;
using Microsoft.Maui.Controls;

namespace HueControllerApp.ViewModels
{
    public class ConnectViewModel : BaseViewModel
    {
        private readonly HueBridgeApiClient _apiClient;

        public string BridgeIp { get; set; }
        public string ConnectionStatus { get; set; }

        public ICommand ConnectCommand { get; }

        public ConnectViewModel()
        {
            _apiClient = new HueBridgeApiClient();
            ConnectCommand = new Command(async () => await ConnectToBridge());
        }

        private async Task ConnectToBridge()
        {
            try
            {
                ConnectionStatus = "Connecting...";
                OnPropertyChanged(nameof(ConnectionStatus));

                var apiKey = await _apiClient.RegisterAppAsync(BridgeIp, "my_hue_app");
                ConnectionStatus = "Connected! API Key: " + apiKey;

                SecureStorage.SetAsync("BridgeIp", BridgeIp);
                SecureStorage.SetAsync("ApiKey", apiKey);

                await Shell.Current.GoToAsync("MainPage");
            }
            catch (Exception ex)
            {
                ConnectionStatus = "Error: " + ex.Message;
            }

            OnPropertyChanged(nameof(ConnectionStatus));
        }


        private void SaveConnectionDetails(string ip, string apiKey)
        {
            SecureStorage.SetAsync("BridgeIp", ip);
            SecureStorage.SetAsync("ApiKey", apiKey);
        }
    }
}
