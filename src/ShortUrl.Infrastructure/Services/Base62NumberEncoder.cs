using System.Text;
using ShortUrl.Application.Interfaces;

namespace ShortUrl.Infrastructure.Services;

public class Base62NumberEncoder : INumberEncoder {
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int Base = 62;

    public string Encode(int number)
    {
        var stringBuilder = new StringBuilder();

        while (number > 0)
        {
            stringBuilder.Append(Alphabet[number % Base]);
            number /= Base;
        }

        return new string(stringBuilder.ToString().Reverse().ToArray());
    }

    public int Decode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Value can't be null or empty");

        int number = 0;

        foreach (char ch in value)
        {
            number = number * Base + Alphabet.IndexOf(ch);
        }

        return number;
    }
}
