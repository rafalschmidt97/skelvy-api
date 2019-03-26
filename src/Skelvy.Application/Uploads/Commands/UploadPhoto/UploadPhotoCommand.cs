using System.IO;
using MediatR;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommand : IRequest<string>
  {
    public string Name { get; set; }
    public Stream Data { get; set; }
    public string ServerPath { get; set; }
  }
}
