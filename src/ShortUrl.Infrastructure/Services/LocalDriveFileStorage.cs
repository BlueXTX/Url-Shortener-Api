using Microsoft.Extensions.Options;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Options;

namespace ShortUrl.Infrastructure.Services;

public class LocalDriveFileStorage : IFileStorage {

    private readonly LocalDriveFileStorageOptions _options;
    private string BasePath => _options.UseRelativePath
        ? Path.Join(AppContext.BaseDirectory, _options.BasePath)
        : _options.BasePath;

    public LocalDriveFileStorage(IOptions<LocalDriveFileStorageOptions> options)
    {
        _options = options.Value;
        EnsureDirectoryCreated();
    }

    private void EnsureDirectoryCreated()
    {
        if (!Directory.Exists(BasePath)) Directory.CreateDirectory(BasePath);
    }

    public async Task Write(string fileName, Stream data)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException($"\"{fileName}\" is not valid file name");

        await using var fileStream = File.Create(Path.Join(BasePath, fileName));
        await data.CopyToAsync(fileStream);
    }

    public Task<Stream> Read(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException($"\"{fileName}\" is not valid file name");

        string filePath = Path.Join(BasePath, fileName);
        if (!File.Exists(filePath)) return Task.FromResult(Stream.Null);

        var fileStream = File.OpenRead(filePath);
        return Task.FromResult<Stream>(fileStream);
    }

    public Task Delete(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException($"\"{fileName}\" is not valid file name");

        string filePath = Path.Join(BasePath, fileName);
        if (!File.Exists(filePath)) return Task.CompletedTask;

        File.Delete(Path.Join(BasePath, fileName));
        return Task.CompletedTask;
    }
}
