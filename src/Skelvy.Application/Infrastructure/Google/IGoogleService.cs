using System.Threading.Tasks;
using Skelvy.Application.Auth.Commands;

namespace Skelvy.Application.Infrastructure.Google
{
  public interface IGoogleService
  {
    Task<T> GetBody<T>(string path, string accessToken, string args = null);
    Task<T> PostBody<T>(string path, string accessToken, object data, string args = null);
    Task<AccessVerification> Verify(string accessToken);
  }
}
