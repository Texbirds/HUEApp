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

            LoadLights();
        }

        private async void LoadLights()
        {
            Lights = new ObservableCollection<Light>
            {
                new Light { Id = "1", Name = "Living Room", IsOn = true, Brightness = 200, Hue = 5000, Saturation = 150 },
                new Light { Id = "2", Name = "Bedroom", IsOn = false, Brightness = 100, Hue = 10000, Saturation = 200 },
                new Light { Id = "3", Name = "Kitchen", IsOn = true, Brightness = 254, Hue = 20000, Saturation = 100 }
            };

            foreach (var light in Lights)
            {
                light.PropertyChanged += async (sender, args) =>
                {
                    if (sender is Light changedLight)
                    {
                        switch (args.PropertyName)
                        {
                            case nameof(Light.IsOn):
                                await _apiClient.ToggleLight("192.168.1.179", "your-api-key", changedLight.Id, changedLight.IsOn);
                                break;
                            case nameof(Light.Brightness):
                                await _apiClient.SetBrightness("192.168.1.179", "your-api-key", changedLight.Id, changedLight.Brightness);
                                break;
                            case nameof(Light.Hue):
                                await _apiClient.SetColor("192.168.1.179", "your-api-key", changedLight.Id, changedLight.Hue, changedLight.Saturation);
                                break;
                            case nameof(Light.Saturation):
                                await _apiClient.SetColor("192.168.1.179", "your-api-key", changedLight.Id, changedLight.Hue, changedLight.Saturation);
                                break;
                        }
                    }
                };
            }
        }

        private async void ApplySelectedPattern()
        {
            if (SelectedPattern == null) return;

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
                    _apiClient.ToggleLight("192.168.1.179", "your-api-key", light.Id, light.IsOn),
                    _apiClient.SetBrightness("192.168.1.179", "your-api-key", light.Id, light.Brightness),
                    _apiClient.SetColor("192.168.1.179", "your-api-key", light.Id, light.Hue, light.Saturation)
                );
            }
        }
    }
}
