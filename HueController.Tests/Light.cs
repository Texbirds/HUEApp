using HueController.Domain.Models;
using Xunit;

namespace HueController.Tests.Domain
{
    public class LightTests
    {
        [Fact]
        public void Light_Should_Initialize_With_Properties()
        {
            var light = new Light
            {
                Id = "1",
                Name = "Living Room Light",
                IsOn = true,
                Brightness = 200,
                Hue = 5000,
                Saturation = 150
            };

            Assert.Equal("1", light.Id);
            Assert.Equal("Living Room Light", light.Name);
            Assert.True(light.IsOn);
            Assert.Equal(200, light.Brightness);
            Assert.Equal(5000, light.Hue);
            Assert.Equal(150, light.Saturation);
        }

        [Fact]
        public void Light_Should_Trigger_PropertyChanged_When_IsOn_Changes()
        {
            var light = new Light();
            bool eventTriggered = false;
            light.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Light.IsOn))
                    eventTriggered = true;
            };

            light.IsOn = true;

            Assert.True(eventTriggered);
        }

        [Fact]
        public void Light_Should_Trigger_PropertyChanged_When_Brightness_Changes()
        {
            var light = new Light();
            bool eventTriggered = false;
            light.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Light.Brightness))
                    eventTriggered = true;
            };

            light.Brightness = 150;

            Assert.True(eventTriggered);
        }
    }
}
