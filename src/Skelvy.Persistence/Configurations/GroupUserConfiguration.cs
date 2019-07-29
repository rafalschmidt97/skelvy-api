using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
  {
    public void Configure(EntityTypeBuilder<GroupUser> builder)
    {
      builder.HasOne(x => x.Group).WithMany(x => x.Users).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.MeetingRequest).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.RemovedReason).HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
