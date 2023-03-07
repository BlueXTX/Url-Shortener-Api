using FluentAssertions;
using ShortUrl.Infrastructure.Services;
using ShortUrl.UnitTests.Data;

namespace ShortUrl.UnitTests.Services;

public class Base62NumberEncoderTests {
    private readonly Base62NumberEncoder _encoder = new();

    [Fact]
    private void Encode_ResultShouldNotBeNullOrEmpty()
    {
        const int number = 1;
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

    [Fact]
    private void Decode_WithPreviouslyEncodedValue_ShouldBeDecoded()
    {
        const int number = 1;
        string encodedValue = _encoder.Encode(number);
        long actual = _encoder.Decode(encodedValue);
        actual.Should().Be(number);
    }
}
