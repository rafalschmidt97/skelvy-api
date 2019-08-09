using System;
using Skelvy.Application.Core.Mappers;
using Skelvy.Domain.Entities;
using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Messages.Queries
{
  public class MessageDto : ICustomMapping
  {
    public int Id { get; set; }
    public string Type { get; private set; }
    public DateTimeOffset Date { get; set; }
    public string Text { get; set; }
    public string AttachmentUrl { get; set; }
    public string Action { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Message, MessageDto>()
        .ForMember(
          destination => destination.AttachmentUrl,
          options => options.MapFrom(x => x.Attachment != null ? x.Attachment.Url : null));
    }
  }
}
