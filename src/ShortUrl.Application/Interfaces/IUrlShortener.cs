using ShortUrl.Domain.Entities;

namespace ShortUrl.Application.Interfaces;

public interface IUrlShortener {
    Task<ShortLink> ShortenUrl(string url);
}
