using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class ProfilePhotoConfiguration : IEntityTypeConfiguration<ProfilePhoto>
  {
    public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
    {
      builder.HasOne(x => x.Profile).WithMany(x => x.Photos).OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Attachment).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
  }
}
