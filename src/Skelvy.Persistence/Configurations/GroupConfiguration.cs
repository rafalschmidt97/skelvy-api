using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class GroupConfiguration : IEntityTypeConfiguration<Group>
  {
    public void Configure(EntityTypeBuilder<Group> builder)
    {
      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.RemovedReason).HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
