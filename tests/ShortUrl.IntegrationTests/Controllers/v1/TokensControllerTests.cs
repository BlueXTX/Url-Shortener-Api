using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using ShortUrl.Api.Dto;
using ShortUrl.IntegrationTests.Data;
using ShortUrl.IntegrationTests.Factories;
using ShortUrl.IntegrationTests.Options;

namespace ShortUrl.IntegrationTests.Controllers.v1;

[Collection("Main")]
public class TokensControllerTests : IClassFixture<UrlShortenerApiFactory> {

    private readonly HttpClient _client;

    public TokensControllerTests(UrlShortenerApiFactory apiFactory)
    {
        _client = apiFactory.CreateDefaultClient();
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task CreateShortLink_WithValidUrl_ShouldReturnOk(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task CreateShortLink_WithValidUrl_ShouldReturnValidDto(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        var responseDto =
            JsonSerializer.Deserialize<ShortLinkDto>(await response.Content.ReadAsStreamAsync(),
                JsonWebSerializerOptions.Instance);
        response.Should().NotBeNull();

        responseDto?.OriginalUrl.Should().Be(url);
        responseDto?.Token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(InvalidUrls))]
    private async Task CreateShortLink_WithInvalidUrl_ShouldReturnBadRequest(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task CreateShortLinkThenRedirect_WithValidUrl_ShouldRedirect(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        var responseDto =
            JsonSerializer.Deserialize<ShortLinkDto>(await response.Content.ReadAsStreamAsync(),
                JsonWebSerializerOptions.Instance);
        var redirectResponse = await _client.GetAsync($"api/v1.0/Token/{responseDto?.Token}");
        redirectResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    private async Task Redirect_WithNonExistentToken_ShouldReturn()
    {
        var response = await _client.GetAsync("/api/v1.0/Token/-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
