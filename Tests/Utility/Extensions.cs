using System.Text.Json;
using System.Net.Http;

namespace Tests
{
    public static class Extensions
    {
        internal static T ParseAs<T>(this HttpResponseMessage response)
        {
            string content = response.Content?
                .ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
