using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Persistence;
using ShortUrl.Infrastructure.Services;

namespace ShortUrl.Infrastructure;

public static class ServiceCollectionExtension {
    public static IServiceCollection AddShortUrl(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationContext, ApplicationContext>(builder => {
            if (configuration.GetSection("UseInMemoryDatabase").Get<bool>())
                builder.UseInMemoryDatabase("ShortUrl");
            else builder.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        services.AddSingleton<IDistributedCounter, InProcessDistributedCounter>();
        services.AddScoped<INumberEncoder, Base62NumberEncoder>();
        services.AddScoped<IUrlShortener, UrlShortener>();
        return services;
    }
}
