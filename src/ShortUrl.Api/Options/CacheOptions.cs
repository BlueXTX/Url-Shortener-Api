namespace ShortUrl.Api.Options;

public record CacheOptions {
    public const string SectionName = "Cache";
    public int MaxSize { get; init; } = 512;
    public bool CacheTokens { get; init; } = true;
    public int CacheTokensTime { get; init; } = 60;
    public bool CacheImages { get; init; } = true;
    public int CacheImagesTime { get; init; } = 30;
    public bool SaveImagesToDrive { get; init; } = true;
}
