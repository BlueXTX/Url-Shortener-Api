using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class InProcessDistributedCounter : IDistributedCounter {
    private int _counter;

    public InProcessDistributedCounter(IApplicationContext context)
    {
        _counter = context.ShortLinks
            .DefaultIfEmpty()
            .Max(x => x == null ? 0 : x.Id);
    }

    public Task<int> Get()
    {
        return Task.FromResult(_counter++);
    }
}
