using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public abstract class BaseController : ControllerBase
  {
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    protected int UserId => int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
  }
}
