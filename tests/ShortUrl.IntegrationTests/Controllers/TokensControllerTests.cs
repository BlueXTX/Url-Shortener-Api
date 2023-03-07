using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ShortUrl.Api;
using ShortUrl.Api.Dto;
using ShortUrl.IntegrationTests.Data;
using ShortUrl.IntegrationTests.Factories;
using ShortUrl.IntegrationTests.Options;

namespace ShortUrl.IntegrationTests.Controllers;

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
        var response = await _client.PostAsJsonAsync("/", dto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task CreateShortLink_WithValidUrl_ShouldReturnValidDto(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("/", dto);
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
        var response = await _client.PostAsJsonAsync("/", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task CreateShortLinkThenRedirect_WithValidUrl_ShouldRedirect(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var response = await _client.PostAsJsonAsync("/", dto);
        var responseDto =
            JsonSerializer.Deserialize<ShortLinkDto>(await response.Content.ReadAsStreamAsync(),
                JsonWebSerializerOptions.Instance);
        var redirectResponse = await _client.GetAsync(responseDto?.Token);
        redirectResponse.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    private async Task Redirect_WithNonExistentToken_ShouldReturn()
    {
        var response = await _client.GetAsync("-1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
