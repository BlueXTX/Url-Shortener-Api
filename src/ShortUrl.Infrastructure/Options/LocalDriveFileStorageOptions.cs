namespace ShortUrl.Infrastructure.Options;

public record LocalDriveFileStorageOptions {
    public const string SectionName = "LocalDriveFileStorage";
    public bool UseRelativePath { get; init; } = true;
    public string BasePath { get; init; } = string.Empty;
}
