using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class Base62NumberEncoder : INumberEncoder {

    public string Encode(int number)
    {
        throw new NotImplementedException();
    }

    public int Decode(string value)
    {
        throw new NotImplementedException();
    }
}
