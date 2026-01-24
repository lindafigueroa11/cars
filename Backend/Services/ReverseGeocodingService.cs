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

        public async Task<(string street, string city)> GetAddressAsync(
            double lat, double lon)
        {
            try
            {
                var url =
                    $"https://nominatim.openstreetmap.org/reverse" +
                    $"?format=jsonv2&lat={lat}&lon={lon}";

                _http.DefaultRequestHeaders.UserAgent.ParseAdd("cars-app");

                var res = await _http.GetAsync(url);
                if (!res.IsSuccessStatusCode)
                    return ("", "");

                using var stream = await res.Content.ReadAsStreamAsync();
                using var json = await JsonDocument.ParseAsync(stream);

                if (!json.RootElement.TryGetProperty("address", out var addr))
                    return ("", "");

                string Get(string name) =>
                    addr.TryGetProperty(name, out var v) ? v.GetString() ?? "" : "";

                return (
                    Get("road"),
                    Get("city")
                );
            }
            catch
            {
                return ("", "");
            }
        }
    }
}
