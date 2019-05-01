using System.IO;
using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Uploads.Commands.UploadPhoto;
using Skelvy.Application.Uploads.Infrastructure.LocalUploads;
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
      var request = new UploadPhotoCommand("photo.jpg", Stream.Null, "localhost");
      var handler = new UploadPhotoCommandHandler(_uploadService.Object);

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new UploadPhotoCommand("photo.jppg", Stream.Null, "localhost");
      _uploadService
        .Setup(x => x.Upload(It.IsAny<Stream>(), It.IsAny<string>()))
        .Throws<BadRequestException>();
      var handler = new UploadPhotoCommandHandler(_uploadService.Object);

      await Assert.ThrowsAsync<BadRequestException>(() =>
        handler.Handle(request));
    }
  }
}
