using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Domain.Services
{
    public interface IBridgeService
    {
        Task<bool> ConnectToBridge(string ipAddress, string deviceType);
        Task<string> GetApiKey();
    }
}
