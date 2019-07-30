using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MessageConfiguration : IEntityTypeConfiguration<Message>
  {
    public void Configure(EntityTypeBuilder<Message> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.Messages).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Group).WithMany(x => x.Messages).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Attachment).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.Date);
      builder.Property(e => e.Text).HasMaxLength(500);
    }
  }
}
