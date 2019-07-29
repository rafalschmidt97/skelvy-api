using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MessageSent;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Attachments;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : CommandHandlerData<AddMeetingChatMessageCommand, MeetingChatMessageDto>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _meetingChatMessagesRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddMeetingChatMessageCommandHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository meetingChatMessagesRepository,
      IAttachmentsRepository attachmentsRepository,
      IMediator mediator,
      IMapper mapper)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _meetingChatMessagesRepository = meetingChatMessagesRepository;
      _attachmentsRepository = attachmentsRepository;
      _mediator = mediator;
      _mapper = mapper;
    }

    public override async Task<MeetingChatMessageDto> Handle(AddMeetingChatMessageCommand request)
    {
      var meetingUser = await _meetingUsersRepository.FindOneByUserId(request.UserId);

      if (meetingUser == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingUser)}(UserId = {request.UserId}) not found.");
      }

      using (var transaction = _meetingChatMessagesRepository.BeginTransaction())
      {
        Attachment attachment = null;

        if (request.AttachmentUrl != null)
        {
          attachment = new Attachment(AttachmentTypes.Image, request.AttachmentUrl);
          await _attachmentsRepository.Add(attachment);
        }

        var message = new MeetingChatMessage(request.Message, DateTimeOffset.UtcNow, attachment?.Id, meetingUser.UserId, meetingUser.MeetingId);
        await _meetingChatMessagesRepository.Add(message);

        transaction.Commit();

        await _mediator.Publish(new MessageSentEvent(message.Id, message.Message, message.Date, attachment?.Url, message.UserId, message.MeetingId));
        return _mapper.Map<MeetingChatMessageDto>(message);
      }
    }
  }
}
