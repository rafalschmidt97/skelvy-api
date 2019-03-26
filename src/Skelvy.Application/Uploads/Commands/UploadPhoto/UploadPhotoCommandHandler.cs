using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Infrastructure.Uploads;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, string>
  {
    private readonly IUploadService _uploadService;

    public UploadPhotoCommandHandler(IUploadService uploadService)
    {
      _uploadService = uploadService;
    }

    public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
      return await _uploadService.Upload(request.Data, request.Name, request.ServerPath, cancellationToken);
    }
  }
}
