using System.IO;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommand : IQuery<string>
  {
    public UploadPhotoCommand(string name, Stream data)
    {
      Name = name;
      Data = data;
    }

    public string Name { get; set; }
    public Stream Data { get; set; }
  }
}
