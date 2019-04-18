using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingUserConfiguration : IEntityTypeConfiguration<MeetingUser>
  {
    public void Configure(EntityTypeBuilder<MeetingUser> builder)
    {
      builder.HasOne(x => x.MeetingRequest).WithMany().HasForeignKey(x => x.MeetingRequestId).OnDelete(DeleteBehavior.Restrict);
      builder.Property(e => e.Status).IsRequired().HasMaxLength(15);
    }
  }
}
