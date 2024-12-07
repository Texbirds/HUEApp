using HueController.Domain.Models;
using Xunit;

namespace HueController.Tests.Domain
{
    public class LightPatternTests
    {
        [Fact]
        public void LightPattern_Should_Initialize_With_Properties()
        {
            var pattern = new LightPattern
            {
                Name = "Relax Mode",
                Hue = 5000,
                Saturation = 150,
                Brightness = 200,
                IsOn = true
            };

            Assert.Equal("Relax Mode", pattern.Name);
            Assert.Equal(5000, pattern.Hue);
            Assert.Equal(150, pattern.Saturation);
            Assert.Equal(200, pattern.Brightness);
            Assert.True(pattern.IsOn.Value);
        }
    }
}
