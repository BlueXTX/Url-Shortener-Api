namespace ShortUrl.Domain.Entities;

public class ShortLink {
    public int Id { get; }
    public string OriginalUrl { get; }
    public string Token { get; }

    public ShortLink(int id, string originalUrl, string token)
    {
        Id = id;
        OriginalUrl = originalUrl;
        Token = token;
    }
}
