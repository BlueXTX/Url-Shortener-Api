using Microsoft.Extensions.Options;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Options;

namespace ShortUrl.Infrastructure.Services;

public class LocalDriveFileStorage : IFileStorage {

    private readonly LocalDriveFileStorageOptions _options;

    public LocalDriveFileStorage(IOptionsSnapshot<LocalDriveFileStorageOptions> options)
    {
        _options = options.Value;
    }

    public Task Write(string fileName, Stream data)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> Read(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string fileName)
    {
        throw new NotImplementedException();
    }
}
