using System.IO;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommand : IQuery<string>
  {
    public UploadPhotoCommand(string name, Stream data, string serverPath)
    {
      Name = name;
      Data = data;
      ServerPath = serverPath;
    }

    public string Name { get; set; }
    public Stream Data { get; set; }
    public string ServerPath { get; set; }
  }
}
