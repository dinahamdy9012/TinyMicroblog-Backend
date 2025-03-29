
using TinyMicroblog.Shared.UploadService.Models;

namespace TinyMicroblog.Shared.UploadService.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadFile(UploadFileRequestModel uploadFileRequestModel);
        Task<string> ReplaceUploadedFile(UploadFileRequestModel uploadFileRequestModel);
        Task<int> DeleteFile(DeleteFileRequestModel deleteFileRequestModel);
       // Task<string> MoveFile(MoveFileRequestModel moveFileRequestModel);
    }
}
