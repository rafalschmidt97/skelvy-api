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

namespace Skelvy.Application.Meetings.Commands.AddMessage
{
  public class AddMessageCommandHandler : CommandHandlerData<AddMessageCommand, MessageDto>
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

    public override async Task<MessageDto> Handle(AddMessageCommand request)
    {
      var existsUser = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.UserId, request.GroupId);

      if (!existsUser)
      {
        throw new NotFoundException($"Entity {nameof(GroupUser)}(UserId = {request.UserId}, GroupId = {request.GroupId}) not found.");
      }

      using (var transaction = _messagesRepository.BeginTransaction())
      {
        Attachment attachment = null;

        if (request.AttachmentUrl != null)
        {
          attachment = new Attachment(AttachmentTypes.Image, request.AttachmentUrl);
          await _attachmentsRepository.Add(attachment);
        }

        var message = new Message(DateTimeOffset.UtcNow, request.Text, attachment?.Id, request.UserId, request.GroupId);
        await _messagesRepository.Add(message);

        transaction.Commit();

        await _mediator.Publish(new MessageSentEvent(message.Id, message.Text, message.Date, attachment?.Url, message.UserId, message.GroupId));
        return _mapper.Map<MessageDto>(message);
      }
    }
  }
}
