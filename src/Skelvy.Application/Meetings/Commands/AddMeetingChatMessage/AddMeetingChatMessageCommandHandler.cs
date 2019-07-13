using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.AddMeetingChatMessage
{
  public class AddMeetingChatMessageCommandHandler : CommandHandlerData<AddMeetingChatMessageCommand, MeetingChatMessageDto>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _meetingChatMessagesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly INotificationsService _notifications;
    private readonly IMapper _mapper;

    public AddMeetingChatMessageCommandHandler(
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository meetingChatMessagesRepository,
      IUsersRepository usersRepository,
      INotificationsService notifications,
      IMapper mapper)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _meetingChatMessagesRepository = meetingChatMessagesRepository;
      _usersRepository = usersRepository;
      _notifications = notifications;
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

      var sender = await _usersRepository.FindOneWithDetails(message.UserId);
      await BroadcastMessage(message, sender.Profile.Name);
      return _mapper.Map<MeetingChatMessageDto>(message);
    }

    private async Task BroadcastMessage(MeetingChatMessage message, string name)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(message.MeetingId);
      var meetingUsersId = meetingUsers.Where(x => x.UserId != message.UserId).Select(x => x.UserId).ToList();

      await _notifications.BroadcastUserSentMessage(
        new UserSentMessageAction(message.Id, message.Message, message.Date, message.AttachmentUrl, message.UserId, name, message.MeetingId),
        meetingUsersId);
    }
  }
}
