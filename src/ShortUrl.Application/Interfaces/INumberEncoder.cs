namespace ShortUrl.Application.Interfaces;

public interface INumberEncoder {
    string Encode(long number);
    long Decode(string value);
}
