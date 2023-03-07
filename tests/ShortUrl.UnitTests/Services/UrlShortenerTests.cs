﻿using FluentAssertions;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Services;
using ShortUrl.UnitTests.Data;

namespace ShortUrl.UnitTests.Services;

public class UrlShortenerTests {
    private readonly UrlShortener _urlShortener;

    public UrlShortenerTests(IDistributedCounter counter, INumberEncoder encoder, IApplicationContext context)
    {
        _urlShortener = new UrlShortener(counter, encoder, context);
    }

    [Theory]
    [ClassData(typeof(ValidUrls))]
    private async Task ShortenUrl_WithValidUrl_UrlAndTokenShouldNotBeNullOrEmpty(string url)
    {
        var result = await _urlShortener.ShortenUrl(url);

        result.OriginalUrl.Should().NotBeNullOrEmpty();
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(InvalidUrls))]
    private async Task ShortenUrl_WithInvalidUrl_ShouldThrow(string url)
    {
        var act = () => _urlShortener.ShortenUrl(url);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
