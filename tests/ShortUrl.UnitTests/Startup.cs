using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Persistence;
using ShortUrl.Infrastructure.Services;

namespace ShortUrl.UnitTests;

public class Startup {
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<IApplicationContext, ApplicationContext>(builder =>
            builder.UseInMemoryDatabase("UrlShortener"));
        services.AddSingleton<IDistributedCounter, InProcessDistributedCounter>();
        services.AddScoped<INumberEncoder, Base62NumberEncoder>();
    }
}
