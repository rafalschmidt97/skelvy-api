using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class BlockedUserConfiguration : IEntityTypeConfiguration<BlockedUser>
  {
    public void Configure(EntityTypeBuilder<BlockedUser> builder)
    {
      builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.BlockUser).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
