using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
  {
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
      builder.HasOne(x => x.User).WithMany(x => x.Roles).OnDelete(DeleteBehavior.Restrict);
      builder.Property(e => e.Name).IsRequired().HasMaxLength(15);
    }
  }
}
