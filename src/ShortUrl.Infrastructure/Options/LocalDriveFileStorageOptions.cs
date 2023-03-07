namespace ShortUrl.Infrastructure.Options;

public record LocalDriveFileStorageOptions(string BasePath, bool UseRelativePath = true);