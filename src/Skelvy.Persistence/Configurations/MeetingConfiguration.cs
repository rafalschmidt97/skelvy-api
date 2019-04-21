using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
  {
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
      builder.HasOne(x => x.Drink).WithMany().OnDelete(DeleteBehavior.Restrict);
      builder.Property(e => e.RemovedReason).HasMaxLength(15);
    }
  }
}
