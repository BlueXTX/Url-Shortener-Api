using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ShortUrl.Api.Dto;
using ShortUrl.IntegrationTests.Data;

namespace ShortUrl.IntegrationTests.Controllers;

public class QrCodesControllerTests : WebApplicationFactory<Program> {
    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task GenerateQr_WithExistingToken_ContentLengthShouldBePositive(string url)
    {
        var client = CreateDefaultClient();
        var dto = new CreateShortLinkDto(url);
        var createResponse = await client.PostAsJsonAsync("/", dto);
        var responseDto = JsonSerializer.Deserialize<ShortLinkDto>(await createResponse.Content.ReadAsStreamAsync(),
            JsonWebSerializerOptions.Instance);
        var qrResponse = await client.GetAsync($"qr/{responseDto?.Token}");
        var content = await qrResponse.Content.ReadAsStreamAsync();
        content.Length.Should().BePositive();
        qrResponse.Content.Headers.ContentType?.ToString().Should().Be("image/png");
    }

    [Fact]
    private async Task GenerateQrCode_WithNonExistentToken_ShouldReturn404()
    {
        var client = CreateDefaultClient();
        var response = await client.GetAsync("/qr/123");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task GenerateQrCode_WithSameToken_ShouldReturnOk(string url)
    {
        var client = CreateDefaultClient();
        var dto = new CreateShortLinkDto(url);
        var createResponse = await client.PostAsJsonAsync("/", dto);
        var responseDto = JsonSerializer.Deserialize<ShortLinkDto>(await createResponse.Content.ReadAsStreamAsync(),
            JsonWebSerializerOptions.Instance);
        var qrResponse = await client.GetAsync($"qr/{responseDto?.Token}");
        var secondQrResponse = await client.GetAsync($"qr/{responseDto?.Token}");

        qrResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondQrResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
