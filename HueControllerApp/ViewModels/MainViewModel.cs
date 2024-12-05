using HueController.Domain.Models;
using HueController.Infrastructure.ApiClients;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace HueController.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Light> Lights { get; set; }
        public ObservableCollection<LightPattern> Patterns { get; set; }
        public LightPattern SelectedPattern { get; set; }
        public ICommand ApplyPatternCommand { get; }

        private readonly HueBridgeApiClient _apiClient;
        private string _bridgeIp;
        private string _apiKey;

        public MainViewModel()
        {
            Lights = new ObservableCollection<Light>();
            Patterns = new ObservableCollection<LightPattern>
            {
                new LightPattern { Name = "Party Mode", Hue = 5000, Saturation = 254, Brightness = 200, IsOn = true },
                new LightPattern { Name = "Relax Mode", Hue = 10000, Saturation = 150, Brightness = 100, IsOn = true },
                new LightPattern { Name = "Sleep Mode", IsOn = false }
            };

            _apiClient = new HueBridgeApiClient();

            ApplyPatternCommand = new Command(ApplySelectedPattern);

            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                // Retrieve Bridge IP and API Key
                _bridgeIp = await SecureStorage.GetAsync("BridgeIp");
                _apiKey = await SecureStorage.GetAsync("ApiKey");

                if (string.IsNullOrEmpty(_bridgeIp) || string.IsNullOrEmpty(_apiKey))
                {
                    throw new InvalidOperationException("Bridge IP or API Key is missing. Please reconnect.");
                }

                LoadLights();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization error: {ex.Message}");
            }
        }

        private async void LoadLights()
        {
            try
            {
                Lights.Clear();

                // Fetch lights from the bridge
                var lights = await _apiClient.GetLights(_bridgeIp, _apiKey);

                foreach (var light in lights)
                {
                    light.PropertyChanged += async (sender, args) =>
                    {
                        if (sender is Light changedLight)
                        {
                            switch (args.PropertyName)
                            {
                                case nameof(Light.IsOn):
                                    await _apiClient.ToggleLight(_bridgeIp, _apiKey, changedLight.Id, changedLight.IsOn);
                                    break;
                                case nameof(Light.Brightness):
                                    await _apiClient.SetBrightness(_bridgeIp, _apiKey, changedLight.Id, changedLight.Brightness);
                                    break;
                                case nameof(Light.Hue):
                                    await _apiClient.SetColor(_bridgeIp, _apiKey, changedLight.Id, changedLight.Hue, changedLight.Saturation);
                                    break;
                                case nameof(Light.Saturation):
                                    await _apiClient.SetColor(_bridgeIp, _apiKey, changedLight.Id, changedLight.Hue, changedLight.Saturation);
                                    break;
                            }
                        }
                    };
                    Lights.Add(light);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading lights: {ex.Message}");
            }
        }

        private async void ApplySelectedPattern()
        {
            if (SelectedPattern == null) return;

            try
            {
                foreach (var light in Lights)
                {
                    if (SelectedPattern.IsOn.HasValue)
                        light.IsOn = SelectedPattern.IsOn.Value;

                    if (SelectedPattern.Brightness.HasValue)
                        light.Brightness = SelectedPattern.Brightness.Value;

                    if (SelectedPattern.Hue.HasValue)
                        light.Hue = SelectedPattern.Hue.Value;

                    if (SelectedPattern.Saturation.HasValue)
                        light.Saturation = SelectedPattern.Saturation.Value;

                    await Task.WhenAll(
                        _apiClient.ToggleLight(_bridgeIp, _apiKey, light.Id, light.IsOn),
                        _apiClient.SetBrightness(_bridgeIp, _apiKey, light.Id, light.Brightness),
                        _apiClient.SetColor(_bridgeIp, _apiKey, light.Id, light.Hue, light.Saturation)
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying pattern: {ex.Message}");
            }
        }
    }
}
