using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingRequestActivityConfiguration : IEntityTypeConfiguration<MeetingRequestActivity>
  {
    public void Configure(EntityTypeBuilder<MeetingRequestActivity> builder)
    {
      builder.HasKey(x => new { x.MeetingRequestId, x.ActivityId });

      builder.HasOne(x => x.MeetingRequest).WithMany(x => x.Activities).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Activity).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
  }
}
