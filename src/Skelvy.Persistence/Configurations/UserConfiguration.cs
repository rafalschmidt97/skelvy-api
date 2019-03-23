using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
      builder.Property(e => e.Language).IsRequired().HasMaxLength(15);
      builder.Property(e => e.FacebookId).HasMaxLength(50);
      builder.Property(e => e.GoogleId).HasMaxLength(50);
    }
  }
}
