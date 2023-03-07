using ShortUrl.Application.Interfaces;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Infrastructure.Services;

public class UrlShortener : IUrlShortener {

    private readonly IDistributedCounter _counter;
    private readonly INumberEncoder _encoder;
    private readonly IApplicationContext _context;

    public UrlShortener(IDistributedCounter counter, INumberEncoder encoder, IApplicationContext context)
    {
        _counter = counter;
        _encoder = encoder;
        _context = context;
    }

    public async Task<ShortLink> ShortenUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Url can't be null or empty");

        int id = await _counter.Get();
        string token = _encoder.Encode(id);
        var shortLink = new ShortLink(id, url, token);
        
        await _context.ShortLinks.AddAsync(shortLink);
        await _context.SaveChangesAsync();
        
        return shortLink;
    }
}
