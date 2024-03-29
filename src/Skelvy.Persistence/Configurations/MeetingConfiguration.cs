﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
  {
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
      builder.HasOne(x => x.Activity).WithMany().OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Group).WithMany().OnDelete(DeleteBehavior.Restrict);

      builder.HasIndex(e => e.IsRemoved);
      builder.HasIndex(e => e.Date);
      builder.HasIndex(e => e.Latitude);
      builder.HasIndex(e => e.Longitude);
      builder.HasIndex(e => e.Size);
      builder.HasIndex(e => e.IsPrivate);
      builder.HasIndex(e => e.IsHidden);

      builder.Property(e => e.Description).HasMaxLength(500);
      builder.Property(e => e.RemovedReason).HasMaxLength(15);
      builder.Property(e => e.ModifiedAt).IsConcurrencyToken();
    }
  }
}
