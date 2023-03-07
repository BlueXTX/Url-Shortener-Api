namespace ShortUrl.Infrastructure.Options;

public record QrCodeGenerationOptions {
    public const string SectionName = "QrCodeGeneration";
    public string BaseUrl { get; init; } = string.Empty;
}
