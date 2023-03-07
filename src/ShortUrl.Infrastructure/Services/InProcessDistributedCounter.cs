using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class InProcessDistributedCounter : IDistributedCounter {
    
    private int _counter;
    
    public InProcessDistributedCounter(int initialValue = 0) => _counter = initialValue;
    public Task<int> Get() => Task.FromResult(Interlocked.Increment(ref _counter));
}
