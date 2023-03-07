using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortUrl.Infrastructure;

namespace ShortUrl.UnitTests.Extensions;

public class ServiceCollectionExtensionTests {

    [Fact]
    private void AddShortUrl_WithValidConfiguration_ShouldWork()
    {
        var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();
        var act = () => services.AddShortUrl(configuration);
        act.Should().NotThrow();
    }
}
