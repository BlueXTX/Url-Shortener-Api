namespace ShortUrl.Application.Interfaces;

public interface IDistributedCounter {
    Task<int> Get();
}
