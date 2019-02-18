using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
  {
    public void Configure(EntityTypeBuilder<Drink> builder)
    {
      builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
    }
  }
}
