﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingUsersConfiguration : IEntityTypeConfiguration<MeetingUser>
  {
    public void Configure(EntityTypeBuilder<MeetingUser> builder)
    {
    }
  }
}