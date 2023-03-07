using Microsoft.Extensions.Options;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Options;

namespace ShortUrl.Infrastructure.Services;

public class LocalDriveFileStorage : IFileStorage {

    private readonly LocalDriveFileStorageOptions _options;
    private string BasePath => _options.UseRelativePath
        ? Path.Join(AppContext.BaseDirectory, _options.BasePath)
        : _options.BasePath;

    public LocalDriveFileStorage(IOptionsSnapshot<LocalDriveFileStorageOptions> options)
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

        var fileStream = File.OpenRead(Path.Join(BasePath, fileName));
        return Task.FromResult<Stream>(fileStream);
    }

    public Task Delete(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException($"\"{fileName}\" is not valid file name");

        File.Delete(Path.Join(BasePath, fileName));
        return Task.CompletedTask;
    }
}
