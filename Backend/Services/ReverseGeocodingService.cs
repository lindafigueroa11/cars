using System.Net.Http;
using System.Text.Json;

namespace Backend.Services
{
    public class ReverseGeocodingService
    {
        private readonly HttpClient _http;

        public ReverseGeocodingService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(string Street, string City)> GetAddressAsync(
            double latitude,
            double longitude
        )
        {
            var url =
                $"https://nominatim.openstreetmap.org/reverse" +
                $"?format=jsonv2&lat={latitude}&lon={longitude}&addressdetails=1";

            _http.DefaultRequestHeaders.UserAgent.ParseAdd("cars-app/1.0");

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            if (!json.RootElement.TryGetProperty("address", out var address))
                return ("", "");

            string Get(params string[] names)
            {
                foreach (var name in names)
                {
                    if (address.TryGetProperty(name, out var v))
                    {
                        var value = v.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                            return value;
                    }
                }
                return "";
            }

            var street = Get("road", "pedestrian", "residential");
            var city = Get("city", "town", "municipality", "village");

            return (street, city);
        }
    }
}
