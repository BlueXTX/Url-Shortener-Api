using ShortUrl.Application.Interfaces;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Infrastructure.Services;

public class UrlShortener : IUrlShortener {

    public Task<ShortLink> ShortenUrl(string url)
    {
        throw new NotImplementedException();
    }
}
