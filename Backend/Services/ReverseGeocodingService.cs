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

        public async Task<(string street, string number, string neighborhood, string city)>
            GetAddressAsync(double latitude, double longitude)
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
            {
                return ("", "", "", "");
            }

            string Get(string name) =>
                address.TryGetProperty(name, out var v) ? v.GetString() ?? "" : "";

            return (
                street: Get("road"),
                number: Get("house_number"),
                neighborhood: Get("neighbourhood"),
                city: Get("city") != "" ? Get("city") : Get("town")
            );
        }
    }
}
