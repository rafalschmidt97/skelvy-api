using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Auth.Commands;

namespace Skelvy.Application.Infrastructure.Facebook
{
  public interface IFacebookService
  {
    Task<T> GetBody<T>(string path, string accessToken, string args, CancellationToken cancellationToken);
    Task<T> PostBody<T>(string path, string accessToken, object data, string args, CancellationToken cancellationToken);
    Task<AccessVerification> Verify(string accessToken, CancellationToken cancellationToken);
  }
}
