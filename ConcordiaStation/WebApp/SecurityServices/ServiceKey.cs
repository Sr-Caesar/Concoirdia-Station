using ConcordiaStation.WebApp.SecurityServices.Interfaces;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace ConcordiaStation.WebApp.SecurityServices
{
    public class ServiceKey : IServiceKey
    {
        public static string GetKey()
        {
            var appSettingsPath = "appsettings.json";
            var appSettingsJson = File.ReadAllText(appSettingsPath);
            var appSettings = JObject.Parse(appSettingsJson);
            var key = appSettings["AppSettings"]["JwtSecretKey"].Value<string>();
            return key;
        }

        public static void SetKey()
        {
            var key = GenerateKey();

            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var appSettingsJson = File.ReadAllText(appSettingsPath);
            var appSettings = JObject.Parse(appSettingsJson);
            appSettings["AppSettings"]["JwtSecretKey"] = key;
            File.WriteAllText(appSettingsPath, appSettings.ToString());
        }

        private static string GenerateKey()
        {
            byte[] keyBytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            return Convert.ToBase64String(keyBytes);
        }
    }
}
