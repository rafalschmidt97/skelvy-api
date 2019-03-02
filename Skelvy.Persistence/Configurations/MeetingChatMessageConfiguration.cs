using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingChatMessageConfiguration : IEntityTypeConfiguration<MeetingChatMessage>
  {
    public void Configure(EntityTypeBuilder<MeetingChatMessage> builder)
    {
      builder.Property(e => e.Message).IsRequired().HasMaxLength(500);
    }
  }
}
