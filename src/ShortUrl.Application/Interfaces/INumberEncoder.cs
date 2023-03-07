namespace ShortUrl.Application.Interfaces;

public interface INumberEncoder {
    string Encode(int number);
    int Decode(string value);
}
