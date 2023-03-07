using FluentAssertions;
using ShortUrl.Infrastructure.Services;
using ShortUrl.UnitTests.Data;

namespace ShortUrl.UnitTests.Services;

public class QrCodeGeneratorTests {
    private readonly DefaultQrCodeGenerator _generator = new();

    [Theory]
    [ClassData(typeof(ValidStrings))]
    private void Generate_WithValidString_BytesShouldNotBeEmpty(string data)
    {
        byte[] actual = _generator.Generate(data);
        actual.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(InvalidStrings))]
    private void Generate_WithInvalidString_ShouldThrow(string data)
    {
        var act = () => _generator.Generate(data);
        act.Should().Throw<ArgumentException>();
    }
}
