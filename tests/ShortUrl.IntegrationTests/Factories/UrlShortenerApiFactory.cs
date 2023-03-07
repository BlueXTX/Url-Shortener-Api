using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShortUrl.Api;
using ShortUrl.Application.Interfaces;
using ShortUrl.Infrastructure.Persistence;

namespace ShortUrl.IntegrationTests.Factories;

public class UrlShortenerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime {
    private readonly PostgreSqlTestcontainer _dbContainer = new ContainerBuilder<PostgreSqlTestcontainer>()
        .WithImage("postgres:latest")
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "ShortUrl",
            Username = "admin",
            Password = "admin"
        })
        .WithCleanUp(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => {
            services.RemoveAll(typeof(IApplicationContext));
            services.AddDbContext<IApplicationContext, ApplicationContext>(connectionBuilder =>
                connectionBuilder.UseNpgsql(_dbContainer.ConnectionString));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}
