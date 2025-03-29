using Upload.API.Application.Models;

namespace Upload.API.Application.Interfaces
{
    public interface IUploadImageService
    {
        Task<UploadPostImageResponseModel> UploadPostImageAsync(IFormFile imageFile, string? existingFileId);
    }
}
