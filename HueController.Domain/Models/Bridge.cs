using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Domain.Models
{
    public class Bridge
    {
        public string Id { get; set; } 
        public string IpAddress { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; } 
    }
}
