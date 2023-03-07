using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class Base62NumberEncoder : INumberEncoder {

    public string Encode(long number)
    {
        throw new NotImplementedException();
    }

    public long Decode(string value)
    {
        throw new NotImplementedException();
    }
}
