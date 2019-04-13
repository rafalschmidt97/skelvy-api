using System.Threading.Tasks;
using Skelvy.Application.Auth.Commands;

namespace Skelvy.Application.Auth.Infrastructure.Google
{
  public interface IGoogleService
  {
    Task<T> GetBody<T>(string path, string accessToken, string args);
    Task<T> PostBody<T>(string path, string accessToken, object data, string args);
    Task<AccessVerification> Verify(string accessToken);
  }
}
