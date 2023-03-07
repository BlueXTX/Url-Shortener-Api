using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Infrastructure.Configuration.Entities;

public class ShortLinkConfiguration : IEntityTypeConfiguration<ShortLink> {

    public void Configure(EntityTypeBuilder<ShortLink> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Token).IsRequired();
        builder.Property(x => x.OriginalUrl).IsRequired();
    }
}
