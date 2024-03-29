using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Messages.Events.MessageSent;
using Skelvy.Application.Messages.Infrastructure.Repositories;
using Skelvy.Application.Messages.Queries;
using Skelvy.Application.Notifications;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Messages.Commands.AddMessage
{
  public class AddMessageCommandHandler : CommandHandlerData<AddMessageCommand, IList<MessageDto>>
  {
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddMessageCommandHandler(
      IGroupUsersRepository groupUsersRepository,
      IMessagesRepository messagesRepository,
      IAttachmentsRepository attachmentsRepository,
      IMediator mediator,
      IMapper mapper)
    {
      _groupUsersRepository = groupUsersRepository;
      _messagesRepository = messagesRepository;
      _attachmentsRepository = attachmentsRepository;
      _mediator = mediator;
      _mapper = mapper;
    }

    public override async Task<IList<MessageDto>> Handle(AddMessageCommand request)
    {
      var groupUserExists = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.UserId, request.GroupId);

      if (!groupUserExists)
      {
        throw new NotFoundException($"{nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {request.GroupId}) not found.");
      }

      var messages = new List<Message>();

      if (request.Type == MessageType.Action)
      {
        if (IsNonPersistenceMessageAction(request.Action))
        {
          messages.Add(await AddNonPersistenceMessageAction(request));
        }
        else
        {
          messages.Add(await AddPersistenceMessageAction(request));
        }
      }
      else
      {
        messages.AddRange(await AddMessageResponse(request));
      }

      return _mapper.Map<IList<MessageDto>>(messages);
    }

    private async Task<Message> AddNonPersistenceMessageAction(AddMessageCommand request)
    {
      var message = new Message(request.Type, DateTimeOffset.UtcNow, null, null, request.Action, request.UserId, request.GroupId);
      await _mediator.Publish(new MessageSentEvent(
        NotificationType.NoPush,
        message.GroupId,
        new MessageSentEventDto(message.Id, message.Type, message.Date, message.Text, null, message.Action, message.UserId, message.GroupId),
        _mapper.Map<IList<MessageDto>>(new List<Message> { message })));
      return message;
    }

    private async Task<Message> AddPersistenceMessageAction(AddMessageCommand request)
    {
      Message message;

      await using var transaction = _messagesRepository.BeginTransaction();
      if (request.Action == MessageActionType.Seen)
      {
        message = await AddOrUpdateSeenMessage(request.UserId, request.GroupId);
      }
      else
      {
        message = new Message(request.Type, DateTimeOffset.UtcNow, null, null, request.Action, request.UserId, request.GroupId);
        await _messagesRepository.Add(message);
      }

      transaction.Commit();
      await _mediator.Publish(new MessageSentEvent(
        NotificationType.NoPush,
        message.GroupId,
        new MessageSentEventDto(message.Id, message.Type, message.Date, message.Text, null, message.Action, message.UserId, message.GroupId),
        _mapper.Map<IList<MessageDto>>(new List<Message> { message })));

      return message;
    }

    private async Task<Message> AddOrUpdateSeenMessage(int userId, int groupId)
    {
      Message message;
      var existingSeenMessage =
        await _messagesRepository.FindOneByActionAndUserIdAndGroupId(MessageActionType.Seen, userId, groupId);

      if (existingSeenMessage != null)
      {
        existingSeenMessage.UpdateSeen();
        await _messagesRepository.Update(existingSeenMessage);
        message = existingSeenMessage;
      }
      else
      {
        message = new Message(MessageType.Action, DateTimeOffset.UtcNow, null, null, MessageActionType.Seen, userId, groupId);
        await _messagesRepository.Add(message);
      }

      return message;
    }

    private async Task<IList<Message>> AddMessageResponse(AddMessageCommand request)
    {
      var messages = new List<Message>();
      Attachment attachment = null;

      await using var transaction = _messagesRepository.BeginTransaction();
      if (request.AttachmentUrl != null)
      {
        attachment = new Attachment(AttachmentType.Image, request.AttachmentUrl);
        await _attachmentsRepository.Add(attachment);
      }

      var message = new Message(request.Type, DateTimeOffset.UtcNow, request.Text, attachment?.Id, null, request.UserId, request.GroupId);
      await _messagesRepository.Add(message);
      var seenMessage = await AddOrUpdateSeenMessage(message.UserId, message.GroupId);

      transaction.Commit();

      await _mediator.Publish(new MessageSentEvent(
        NotificationType.Regular,
        message.GroupId,
        new MessageSentEventDto(message.Id, message.Type, message.Date, message.Text, attachment?.Url, message.Action, message.UserId, message.GroupId),
        _mapper.Map<IList<MessageDto>>(new List<Message> { message, seenMessage })));

      messages.Add(message);
      messages.Add(seenMessage);

      return messages;
    }

    private static bool IsNonPersistenceMessageAction(string action)
    {
      return action == MessageActionType.TypingOn ||
             action == MessageActionType.TypingOff;
    }
  }
}
