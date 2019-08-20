using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingInvitationConfiguration : IEntityTypeConfiguration<MeetingInvitation>
  {
    public void Configure(EntityTypeBuilder<MeetingInvitation> builder)
    {
      builder.HasOne(x => x.InvitingUser).WithMany(x => x.MeetingInvitations).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.Status).IsRequired().HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
