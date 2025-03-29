using MediatR;
using Upload.API.Application.Interfaces;
using Upload.API.Application.Models;

namespace Upload.API.Application.UseCases.Upload
{
    public class UploadPostImageCommandHandler : IRequestHandler<UploadPostImageCommand, UploadPostImageResponseModel>
    {
        private readonly IUploadImageService _uploadImageService;

        public UploadPostImageCommandHandler(IUploadImageService uploadImageService)
        {
            _uploadImageService = uploadImageService ?? throw new ArgumentNullException(nameof(uploadImageService));
        }
        public async Task<UploadPostImageResponseModel> Handle(UploadPostImageCommand request, CancellationToken cancellationToken)
        {
            return await _uploadImageService.UploadPostImageAsync(request.ImageFile, request.FileId);
        }
    }
}
