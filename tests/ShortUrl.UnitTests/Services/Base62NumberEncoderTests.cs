using FluentAssertions;
using ShortUrl.Infrastructure.Services;
using ShortUrl.UnitTests.Data;

namespace ShortUrl.UnitTests.Services;

public class Base62NumberEncoderTests {
    private readonly Base62NumberEncoder _encoder = new();

    [Theory]
    [ClassData(typeof(TestNumbers))]
    private void Encode_ResultShouldNotBeNullOrEmpty(int number)
    {
        string actual = _encoder.Encode(number);
        actual.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(InvalidStrings))]
    private void Decode_WithInvalidString_ShouldThrow(string encodedValue)
    {
        var act = () => _encoder.Decode(encodedValue);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [ClassData(typeof(TestNumbers))]
    private void Decode_WithPreviouslyEncodedValue_ShouldBeDecoded(int number)
    {
        string encodedValue = _encoder.Encode(number);
        long actual = _encoder.Decode(encodedValue);
        actual.Should().Be(number);
    }
}
