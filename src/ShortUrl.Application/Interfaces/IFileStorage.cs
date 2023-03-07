namespace ShortUrl.Application.Interfaces;

public interface IFileStorage {
    Task Write(string fileName, Stream data);
    Task<Stream> Read(string fileName);
    Task Delete(string fileName);
}
