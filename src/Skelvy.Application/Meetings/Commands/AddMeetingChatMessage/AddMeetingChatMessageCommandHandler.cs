using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MessageSent;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : CommandHandlerData<AddMeetingChatMessageCommand, MeetingChatMessageDto>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _meetingChatMessagesRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddMeetingChatMessageCommandHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository meetingChatMessagesRepository,
      IMediator mediator,
      IMapper mapper)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _meetingChatMessagesRepository = meetingChatMessagesRepository;
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

      var message = new MeetingChatMessage(request.Message, DateTimeOffset.UtcNow, request.AttachmentUrl, meetingUser.UserId, meetingUser.MeetingId);
      await _meetingChatMessagesRepository.Add(message);

      await _mediator.Publish(new MessageSentEvent(message.Id, message.Message, message.Date, message.AttachmentUrl, message.UserId, message.MeetingId));
      return _mapper.Map<MeetingChatMessageDto>(message);
    }
  }
}
