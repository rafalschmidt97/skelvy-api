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

      builder.HasIndex(e => e.IsRemoved);
      builder.HasIndex(e => e.Status);
      builder.HasIndex(e => e.MinDate);
      builder.HasIndex(e => e.MaxDate);
      builder.HasIndex(e => e.MinAge);
      builder.HasIndex(e => e.MaxAge);
      builder.HasIndex(e => e.Latitude);
      builder.HasIndex(e => e.Longitude);

      builder.Property(e => e.Status).IsRequired().HasMaxLength(15);
      builder.Property(e => e.RemovedReason).HasMaxLength(15);
    }
  }
}
