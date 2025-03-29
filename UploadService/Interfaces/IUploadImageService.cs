
using UploadService.Models;

namespace UploadService.Interfaces
{
    public interface IUploadImageService
    {
        Task<int> UploadFile(UploadFileRequestModel uploadFileRequestModel);
        Task<int> DeleteFile(DeleteFileRequestModel deleteFileRequestModel);
        Task<string> MoveFileToPermanentStorageAsync(string tempFileName, string tempContainer, string permenantContainer);
    }
}
