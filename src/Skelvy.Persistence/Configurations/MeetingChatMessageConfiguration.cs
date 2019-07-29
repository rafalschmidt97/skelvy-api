using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingChatMessageConfiguration : IEntityTypeConfiguration<MeetingChatMessage>
  {
    public void Configure(EntityTypeBuilder<MeetingChatMessage> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.MeetingChatMessages).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Meeting).WithMany(x => x.ChatMessages).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Attachment).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.Date);
      builder.Property(e => e.Message).HasMaxLength(500);
    }
  }
}
