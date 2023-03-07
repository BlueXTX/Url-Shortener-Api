using Microsoft.EntityFrameworkCore;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Application.Interfaces;

public interface IApplicationContext {
    DbSet<ShortLink> ShortLinks { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
