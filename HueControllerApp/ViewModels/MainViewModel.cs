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

        public MainViewModel(string ipAddress, string apiKey)
        {
            _apiClient = new HueBridgeApiClient();
            _bridgeIp = ipAddress;
            _apiKey = apiKey;

            Lights = new ObservableCollection<Light>();
            Patterns = new ObservableCollection<LightPattern>
            {
                new LightPattern { Name = "Party Mode", IsOn = true }, 
                new LightPattern { Name = "Relax Mode", Hue = 10000, Saturation = 150, Brightness = 100, IsOn = true },
                new LightPattern { Name = "Sleep Mode", IsOn = false },
                new LightPattern { Name = "Rainbow", IsOn = true }, 
                new LightPattern { Name = "Warm Evening", Hue = 5000, Saturation = 200, Brightness = 180, IsOn = true },
                new LightPattern { Name = "Cool Morning", Hue = 45000, Saturation = 100, Brightness = 220, IsOn = true }
            };

            ApplyPatternCommand = new Command(ApplySelectedPattern);

            Initialize();
        }

        private async void Initialize()
        {
            try
            {
                if (string.IsNullOrEmpty(_bridgeIp) || string.IsNullOrEmpty(_apiKey))
                {
                    throw new InvalidOperationException("Bridge IP or API Key is missing.");
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
                var random = new Random();
                int[] rainbowHues = { 0, 10000, 20000, 30000, 40000, 50000, 60000 }; 

                for (int i = 0; i < Lights.Count; i++)
                {
                    var light = Lights[i];

                    if (SelectedPattern.IsOn.HasValue)
                        light.IsOn = SelectedPattern.IsOn.Value;

                    if (SelectedPattern.Brightness.HasValue)
                        light.Brightness = SelectedPattern.Brightness.Value;

                    if (SelectedPattern.Hue.HasValue)
                        light.Hue = SelectedPattern.Hue.Value;

                    if (SelectedPattern.Saturation.HasValue)
                        light.Saturation = SelectedPattern.Saturation.Value;

                    if (SelectedPattern.Name == "Party Mode")
                    {
                        light.Hue = random.Next(0, 65535);
                        light.Saturation = random.Next(200, 254);
                        light.Brightness = random.Next(150, 254);
                    }
                    else if (SelectedPattern.Name == "Rainbow")
                    {
                        light.Hue = rainbowHues[i % rainbowHues.Length];
                        light.Saturation = 254;
                        light.Brightness = 200;
                    }

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
