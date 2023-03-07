namespace ShortUrl.Infrastructure.Options;

public record LocalDriveFileStorageOptions {
    public bool UseRelativePath { get; init; } = true;
    public string BasePath { get; init; }
    public const string SectionName = "LocalDriveFileStorageOptions";
}
