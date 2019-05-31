using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingRequestDrinkConfiguration : IEntityTypeConfiguration<MeetingRequestDrinkType>
  {
    public void Configure(EntityTypeBuilder<MeetingRequestDrinkType> builder)
    {
      builder.HasKey(x => new { x.MeetingRequestId, x.DrinkTypeId });

      builder.HasOne(x => x.MeetingRequest).WithMany(x => x.DrinkTypes).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.DrinkType).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
  }
}
