using MediatR;
using Upload.API.Application.Models;

namespace Upload.API.Application.UseCases.Upload
{
    public class UploadPostImageCommand : IRequest<UploadPostImageResponseModel>
    {
        public IFormFile ImageFile { get; }
        public string? FileId { get; }
        public UploadPostImageCommand(IFormFile imageFile, string? fileId)
        {
            ImageFile = imageFile;
            FileId = fileId;
        }
    }
}
