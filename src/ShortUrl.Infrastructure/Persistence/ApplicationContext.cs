using Microsoft.EntityFrameworkCore;
using ShortUrl.Application.Interfaces;
using ShortUrl.Domain.Entities;
using ShortUrl.Infrastructure.Configuration.Entities;

namespace ShortUrl.Infrastructure.Persistence;

public class ApplicationContext : DbContext, IApplicationContext {
    public DbSet<ShortLink> ShortLinks { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShortLinkConfiguration).Assembly);
    }
}
