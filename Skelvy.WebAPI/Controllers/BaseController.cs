using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Skelvy.WebAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BaseController : Controller
  {
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
  }
}
