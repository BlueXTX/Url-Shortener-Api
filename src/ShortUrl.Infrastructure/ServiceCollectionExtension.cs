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

        services.AddSingleton<IDistributedCounter, InProcessDistributedCounter>(provider => {
            using var scope = provider.CreateScope();
            int maxId = scope.ServiceProvider.GetRequiredService<IApplicationContext>().ShortLinks
                .DefaultIfEmpty()
                .Max(x => x == null ? 0 : x.Id);
            return new InProcessDistributedCounter(maxId);
        });

        services.AddScoped<INumberEncoder, Base62NumberEncoder>();
        services.AddScoped<IUrlShortener, UrlShortener>();
        services.AddScoped<IQrCodeGenerator, DefaultQrCodeGenerator>();
        
        return services;
    }
}
