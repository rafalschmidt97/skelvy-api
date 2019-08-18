using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skelvy.Application.Messages.Commands.AddMessage;
using Skelvy.Application.Messages.Queries;
using Skelvy.Application.Messages.Queries.FindMessages;

namespace Skelvy.WebAPI.Controllers
{
  public class MessagesController : BaseController
  {
    [HttpGet]
    public async Task<IList<MessageDto>> Find([FromQuery] FindMessagesQuery request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }

    [HttpPost]
    public async Task<IList<MessageDto>> Add(AddMessageCommand request)
    {
      request.UserId = UserId;
      return await Mediator.Send(request);
    }
  }
}
