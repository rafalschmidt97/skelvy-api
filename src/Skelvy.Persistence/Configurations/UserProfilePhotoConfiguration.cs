﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class UserProfilePhotoConfiguration : IEntityTypeConfiguration<UserProfilePhoto>
  {
    public void Configure(EntityTypeBuilder<UserProfilePhoto> builder)
    {
      builder.Property(e => e.Url).IsRequired().HasMaxLength(2048);
    }
  }
}