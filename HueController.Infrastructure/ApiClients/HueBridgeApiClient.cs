using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HueController.Domain.Models;

namespace HueController.Infrastructure.ApiClients
{
    public class HueBridgeApiClient
    {
        private readonly HttpClient _httpClient;

        public HueBridgeApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> RegisterAppAsync(string bridgeIp, string deviceType)
        {
            var url = $"http://{bridgeIp}/api";
            var body = JsonSerializer.Serialize(new { devicetype = deviceType });

            var response = await _httpClient.PostAsync(url, new StringContent(body));
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var parsed = JsonSerializer.Deserialize<dynamic>(jsonResponse);

            if (parsed?[0]?.success != null)
            {
                return parsed[0].success.username;
            }
            else if (parsed?[0]?.error != null)
            {
                throw new InvalidOperationException(parsed[0].error.description);
            }

            return null;
        }

        public async Task<List<Light>> GetLights(string bridgeIp, string apiKey)
        {
            var url = $"http://{bridgeIp}/api/{apiKey}/lights";
            var response = await _httpClient.GetStringAsync(url);
            var lightsResponse = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(response);

            var lights = new List<Light>();
            foreach (var (id, data) in lightsResponse)
            {
                var state = data.state;
                lights.Add(new Light
                {
                    Id = id,
                    Name = data.name,
                    IsOn = state.on,
                    Brightness = state.bri,
                    Hue = state.hue,
                    Saturation = state.sat
                });
            }

            return lights;
        }

        public async Task<bool> ToggleLight(string bridgeIp, string apiKey, string lightId, bool isOn)
        {
            var url = $"http://{bridgeIp}/api/{apiKey}/lights/{lightId}/state";
            var body = JsonSerializer.Serialize(new { on = isOn });
            var response = await _httpClient.PutAsync(url, new StringContent(body));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetBrightness(string bridgeIp, string apiKey, string lightId, int brightness)
        {
            var url = $"http://{bridgeIp}/api/{apiKey}/lights/{lightId}/state";
            var body = JsonSerializer.Serialize(new { bri = brightness });
            var response = await _httpClient.PutAsync(url, new StringContent(body));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetColor(string bridgeIp, string apiKey, string lightId, int hue, int saturation)
        {
            var url = $"http://{bridgeIp}/api/{apiKey}/lights/{lightId}/state";
            var body = JsonSerializer.Serialize(new { hue = hue, sat = saturation });
            var response = await _httpClient.PutAsync(url, new StringContent(body));
            return response.IsSuccessStatusCode;
        }
    }
}
