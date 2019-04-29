using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingUserConfiguration : IEntityTypeConfiguration<MeetingUser>
  {
    public void Configure(EntityTypeBuilder<MeetingUser> builder)
    {
      builder.HasOne(x => x.Meeting).WithMany(x => x.Users).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.MeetingRequest).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
