using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public abstract class BaseController : Controller
  {
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    protected int UserId => int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
  }
}
