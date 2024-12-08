using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;

namespace HueControllerApp.Converters
{
    public class HueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int hue)
            {
                // Normalize Hue to a range of 0-360 for the color wheel
                float normalizedHue = (float)((hue / 65535.0) * 360);

                // Perform HSV to RGB conversion
                (float r, float g, float b) = HsvToRgb(normalizedHue, 1f, 1f);

                // Return a Color object
                return new Color(r, g, b);
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts HSV values to RGB. Made with ChatGPT.
        /// </summary>
        /// <param name="h">Hue (0-360)</param>
        /// <param name="s">Saturation (0-1)</param>
        /// <param name="v">Value (0-1)</param>
        /// <returns>Tuple of RGB values (0-1)</returns>
        private static (float r, float g, float b) HsvToRgb(float h, float s, float v)
        {
            float c = v * s; 
            float x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            float m = v - c;

            float r = 0, g = 0, b = 0;

            if (h >= 0 && h < 60)
            {
                r = c; g = x; b = 0;
            }
            else if (h >= 60 && h < 120)
            {
                r = x; g = c; b = 0;
            }
            else if (h >= 120 && h < 180)
            {
                r = 0; g = c; b = x;
            }
            else if (h >= 180 && h < 240)
            {
                r = 0; g = x; b = c;
            }
            else if (h >= 240 && h < 300)
            {
                r = x; g = 0; b = c;
            }
            else if (h >= 300 && h <= 360)
            {
                r = c; g = 0; b = x;
            }

            return (r + m, g + m, b + m);
        }
    }
}
