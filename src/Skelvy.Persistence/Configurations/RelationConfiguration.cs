using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class RelationConfiguration : IEntityTypeConfiguration<Relation>
  {
    public void Configure(EntityTypeBuilder<Relation> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.Relations).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.Type).IsRequired().HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
