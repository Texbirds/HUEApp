using System.Globalization;

namespace HueControllerApp.Helpers
{
    public static class LocalizationHelper
    {
        /// <summary>
        /// Sets the culture for the application.
        /// </summary>
        /// <param name="languageCode">Language code (e.g., "en-US" or "nl-NL").</param>
        public static void SetCulture(string languageCode)
        {
            var culture = new CultureInfo(languageCode);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
