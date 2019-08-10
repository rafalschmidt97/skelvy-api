using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
  {
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
      builder.HasOne(x => x.InvitingUser).WithMany(x => x.FriendsRequests).OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);

      builder.Property(e => e.Status).IsRequired().HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
