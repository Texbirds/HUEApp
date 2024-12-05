using HueController.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Domain.Services
{
    public interface ILightService
    {
        Task<List<Light>> GetLights(Bridge bridge);
        Task<bool> ToggleLight(Bridge bridge, string lightId, bool isOn);
        Task<bool> SetBrightness(Bridge bridge, string lightId, int brightness);
        Task<bool> SetColor(Bridge bridge, string lightId, int hue, int saturation);
    }
}
