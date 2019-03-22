using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skelvy.Domain.Entities;

namespace Skelvy.Persistence.Configurations
{
  public class MeetingRequestConfiguration : IEntityTypeConfiguration<MeetingRequest>
  {
    public void Configure(EntityTypeBuilder<MeetingRequest> builder)
    {
    }
  }
}
