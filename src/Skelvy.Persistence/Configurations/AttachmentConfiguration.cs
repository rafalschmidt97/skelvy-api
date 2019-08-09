using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
  {
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
      builder.Property(e => e.Type).IsRequired().HasMaxLength(15);
      builder.Property(e => e.Url).IsRequired().HasMaxLength(2048);
    }
  }
}
