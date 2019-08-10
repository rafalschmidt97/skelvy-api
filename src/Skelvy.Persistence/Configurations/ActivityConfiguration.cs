using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
  {
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
      builder.HasIndex(e => e.Name).IsUnique();

      builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
    }
  }
}
