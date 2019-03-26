using System.Threading.Tasks;

namespace Skelvy.WebAPI.Infrastructure.Emails
{
  public interface IRazorViewToStringRenderer
  {
    Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
  }
}
