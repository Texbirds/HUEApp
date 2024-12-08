using HueController.Domain.Models;

namespace HueController.Tests.Domain
{
    public class BridgeTests
    {
        [Fact]
        public void Bridge_Should_Initialize_With_Properties()
        {
            var bridge = new Bridge
            {
                Id = "1",
                IpAddress = "192.168.1.1",
                ApiKey = "test_api_key",
                Name = "Test Bridge"
            };

            Assert.Equal("1", bridge.Id);
            Assert.Equal("192.168.1.1", bridge.IpAddress);
            Assert.Equal("test_api_key", bridge.ApiKey);
            Assert.Equal("Test Bridge", bridge.Name);
        }
    }
}