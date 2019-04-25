using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingRequestDrinkConfiguration : IEntityTypeConfiguration<MeetingRequestDrink>
  {
    public void Configure(EntityTypeBuilder<MeetingRequestDrink> builder)
    {
      builder.HasKey(x => new { x.MeetingRequestId, x.DrinkId });

      builder.HasOne(x => x.MeetingRequest).WithMany(x => x.Drinks).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Drink).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
  }
}
