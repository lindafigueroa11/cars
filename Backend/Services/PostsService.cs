using System.Text.Json;
using Backend.DTOs;

namespace Backend.Services
{
    public class PostsService : IPostsService
    {
        private readonly HttpClient _httpClient;

        public PostsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<PostsDTOs>> Get()
        {
            var result = await _httpClient.GetAsync("");
            result.EnsureSuccessStatusCode();

            var body = await result.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<IEnumerable<PostsDTOs>>(body, options)
                   ?? Enumerable.Empty<PostsDTOs>();
        }
    }
}
