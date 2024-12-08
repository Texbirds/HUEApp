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
            var body = JsonSerializer.Serialize(new { devicetype = deviceType, generateclientkey = true });

            try
            {
                var response = await _httpClient.PostAsync(url, new StringContent(body));
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                {
                    var responseObject = doc.RootElement[0];

                    if (responseObject.TryGetProperty("success", out var successElement))
                    {
                        if (successElement.TryGetProperty("username", out var usernameElement))
                        {
                            return usernameElement.GetString();
                        }
                    }
                    else if (responseObject.TryGetProperty("error", out var errorElement))
                    {
                        throw new InvalidOperationException(errorElement.GetProperty("description").GetString());
                    }
                }

                throw new InvalidOperationException("Unexpected response from the bridge.");
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Connection failed: {ex.Message}");
            }
        }


        public async Task<List<Light>> GetLights(string bridgeIp, string apiKey)
        {
            var url = $"http://{bridgeIp}/api/{apiKey}/lights";
            var response = await _httpClient.GetStringAsync(url);

            var lightsResponse = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

            var lights = new List<Light>();

            if (lightsResponse != null)
            {
                foreach (var lightEntry in lightsResponse)
                {
                    var id = lightEntry.Key; 
                    var data = lightEntry.Value;

                    var state = data.GetProperty("state");
                    var light = new Light
                    {
                        Id = id,
                        Name = data.GetProperty("name").GetString(),
                        IsOn = state.GetProperty("on").GetBoolean(),
                        Brightness = state.GetProperty("bri").GetInt32(),
                        Hue = state.GetProperty("hue").GetInt32(),
                        Saturation = state.GetProperty("sat").GetInt32()
                    };

                    lights.Add(light);
                }
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
