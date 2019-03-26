using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Infrastructure.Uploads;
using Skelvy.Application.Uploads.Commands.UploadPhoto;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Uploads.Commands
{
  public class UploadPhotoCommandHandlerTest : RequestTestBase
  {
    private readonly Mock<IUploadService> _uploadService;

    public UploadPhotoCommandHandlerTest()
    {
      _uploadService = new Mock<IUploadService>();
    }

    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new UploadPhotoCommand { Name = "photo.jpg", ServerPath = "localhost" };
      var handler = new UploadPhotoCommandHandler(_uploadService.Object);

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new UploadPhotoCommand { Name = "photo.jppg", ServerPath = "localhost" };
      _uploadService
        .Setup(x => x.Upload(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
        .Throws<BadRequestException>();
      var handler = new UploadPhotoCommandHandler(_uploadService.Object);

      await Assert.ThrowsAsync<BadRequestException>(() =>
        handler.Handle(request, CancellationToken.None));
    }
  }
}
