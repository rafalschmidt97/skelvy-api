using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
  {
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.ExpiredAt);
      builder.HasIndex(e => e.Token);

      builder.Property(e => e.Token).IsRequired().HasMaxLength(50);
    }
  }
}
