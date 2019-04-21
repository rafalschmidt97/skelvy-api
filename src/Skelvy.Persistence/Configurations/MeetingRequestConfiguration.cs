using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingRequestConfiguration : IEntityTypeConfiguration<MeetingRequest>
  {
    public void Configure(EntityTypeBuilder<MeetingRequest> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.MeetingRequests).OnDelete(DeleteBehavior.Restrict);
      builder.Property(e => e.Status).IsRequired().HasMaxLength(15);
      builder.Property(e => e.RemovedReason).HasMaxLength(15);
    }
  }
}
