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
public class QrCodesControllerTests : IClassFixture<UrlShortenerApiFactory> {

    private readonly HttpClient _client;

    public QrCodesControllerTests(UrlShortenerApiFactory apiFactory)
    {
        _client = apiFactory.CreateDefaultClient();
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task GenerateQr_WithExistingToken_ContentLengthShouldBePositive(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var createResponse = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        var responseDto = JsonSerializer.Deserialize<ShortLinkDto>(await createResponse.Content.ReadAsStreamAsync(),
            JsonWebSerializerOptions.Instance);
        var qrResponse = await _client.GetAsync($"api/v1.0/qr/{responseDto?.Token}");
        var content = await qrResponse.Content.ReadAsStreamAsync();
        content.Length.Should().BePositive();
        qrResponse.Content.Headers.ContentType?.ToString().Should().Be("image/png");
    }

    [Fact]
    private async Task GenerateQrCode_WithNonExistentToken_ShouldReturn404()
    {
        var response = await _client.GetAsync("api/v1.0/qr/123");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task GenerateQrCode_WithSameToken_ShouldReturnOk(string url)
    {
        var dto = new CreateShortLinkDto(url);
        var createResponse = await _client.PostAsJsonAsync("api/v1.0/Token", dto);
        var responseDto = JsonSerializer.Deserialize<ShortLinkDto>(await createResponse.Content.ReadAsStreamAsync(),
            JsonWebSerializerOptions.Instance);
        var qrResponse = await _client.GetAsync($"api/v1.0/qr/{responseDto?.Token}");
        var secondQrResponse = await _client.GetAsync($"api/v1.0/qr/{responseDto?.Token}");

        qrResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondQrResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
