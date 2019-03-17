using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
  {
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
      builder.Property(e => e.RegistrationId).IsRequired().HasMaxLength(250);
    }
  }
}
