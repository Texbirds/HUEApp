using NUnit.Framework;
using Xamarin.UITest;

namespace HueController.Tests
{
    [TestFixture]
    public class ConnectPageTests
    {
        IApp app;

        [SetUp]
        public void SetUp()
        {
            app = ConfigureApp.Android
                .ApkFile("HueControllerApp\\bin\\Debug\\net8.0-android")
                .StartApp();
        }

        [Test]
        public void HappyPath_ConnectToHueBridge()
        {
            app.EnterText("EntryBridgeIp", "localhost:8000");

            app.Tap("ButtonConnect");

            app.WaitForElement(c => c.Marked("LabelConnectionStatus").Text("Connected!"));
        }

        [Test]
        public void HappyPath_AddBridgeToConnectedList()
        {
            app.EnterText("EntryBridgeIp", "localhost:8000");

            app.Tap("ButtonConnect");

            app.WaitForElement(c => c.Marked("LabelConnectionStatus").Text("Connected!"));

            app.WaitForElement(c => c.Marked("BridgeList").Child(0).Text("localhost:8000"));
        }

        [Test]
        public void UnhappyPath_DuplicateBridgeIp()
        {
            app.EnterText("EntryBridgeIp", "localhost:8000");

            app.Tap("ButtonConnect");

            app.WaitForElement(c => c.Marked("LabelConnectionStatus").Text("Connected!"));

            app.EnterText("EntryBridgeIp", "localhost:8000");

            app.Tap("ButtonConnect");

            app.WaitForElement(c => c.Marked("LabelConnectionStatus").Text("Error: Bridge already connected"));
        }

        [Test]
        public void UnhappyPath_InvalidIpAddress()
        {
            app.EnterText("EntryBridgeIp", "invalid_ip");

            app.Tap("ButtonConnect");

            app.WaitForElement(c => c.Marked("LabelConnectionStatus").Text("Error: Connection failed"));
        }
    }
}
