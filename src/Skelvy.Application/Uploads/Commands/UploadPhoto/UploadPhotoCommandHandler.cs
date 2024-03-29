using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Uploads.Infrastructure.Uploads;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommandHandler : QueryHandler<UploadPhotoCommand, string>
  {
    private readonly IUploadService _uploadService;

    public UploadPhotoCommandHandler(IUploadService uploadService)
    {
      _uploadService = uploadService;
    }

    public override async Task<string> Handle(UploadPhotoCommand request)
    {
      return await _uploadService.Upload(request.Data, request.Name);
    }
  }
}
