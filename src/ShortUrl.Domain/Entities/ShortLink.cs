namespace ShortUrl.Domain.Entities;

public class ShortLink {
    public long Id { get; }
    public string OriginalUrl { get; }
    public string Token { get; }

    public ShortLink(long id, string originalUrl, string token)
    {
        Id = id;
        OriginalUrl = originalUrl;
        Token = token;
    }
}
