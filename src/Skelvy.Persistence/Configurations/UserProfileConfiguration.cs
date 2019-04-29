using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
  {
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
      builder.HasOne(x => x.User).WithOne(x => x.Profile).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.Birthday);
      builder.HasIndex(e => e.Gender);

      builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
      builder.Property(e => e.Gender).IsRequired().HasMaxLength(15);
      builder.Property(e => e.Description).HasMaxLength(500);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
