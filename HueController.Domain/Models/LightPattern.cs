using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueController.Domain.Models
{
    public class LightPattern
    {
        public string Name { get; set; }         
        public int? Hue { get; set; }            
        public int? Saturation { get; set; }     
        public int? Brightness { get; set; }     
        public bool? IsOn { get; set; }      
    }
}
