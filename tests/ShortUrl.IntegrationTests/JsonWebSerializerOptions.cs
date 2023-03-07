using System.Text.Json;

namespace ShortUrl.IntegrationTests;

public class JsonWebSerializerOptions {
    public static readonly JsonSerializerOptions Instance = new(JsonSerializerDefaults.Web);
}
