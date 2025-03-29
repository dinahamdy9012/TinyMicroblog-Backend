using TinyMicroblog.Shared.UploadService.ErrorCodes;
using TinyMicroblog.Shared.UploadService.Interfaces;
using TinyMicroblog.Shared.UploadService.Models;
using TinyMicroblog.SharedKernel.Exceptions;
using Upload.API.Application.Interfaces;
using Upload.API.Application.Models;


namespace Upload.API.Application.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;
        public UploadImageService(IConfiguration configuration, IUploadService uploadService)
        {
            _configuration = configuration;
            _uploadService = uploadService;
        }

        public async Task<UploadPostImageResponseModel> UploadPostImageAsync(IFormFile imageFile, string? existingFileId)
        {
            if (!IsValidExtension(imageFile))
                throw new AppException(System.Net.HttpStatusCode.BadRequest, nameof(UploadErrorCodes.InvalidExtension));

            if (!IsValidSize(imageFile))
                throw new AppException(System.Net.HttpStatusCode.BadRequest, nameof(UploadErrorCodes.InvalidSize));

            string fileId = existingFileId ?? Guid.NewGuid().ToString();
            var ext = Path.GetExtension(imageFile!.FileName);
            var fileName = $"{fileId}{ext}";

            UploadFileToAzureRequestModel uploadFileRequestModel = new UploadFileToAzureRequestModel();
            uploadFileRequestModel.ContainerName = _configuration["AzureContainers:PostsTempContainer"];
            var stream = new MemoryStream();

            imageFile.CopyTo(stream);
            uploadFileRequestModel.FileContent = stream.ToArray();
            uploadFileRequestModel.FileName = fileName;
            var result = await _uploadService.ReplaceUploadedFile(uploadFileRequestModel);
            return new UploadPostImageResponseModel(fileId, fileName, result);

        }

        private bool IsValidExtension(IFormFile file)
        {
            var allowedExtensions = _configuration.GetSection("ImageFileSettings:AllowedExtensions").Get<List<string>>();
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (allowedExtensions != null && allowedExtensions.Contains(extension))
            {
                return true;
            }
            return false;
        }
        private bool IsValidSize(IFormFile file)
        {
            return file.Length <= _configuration.GetSection("ImageFileSettings:AcceptedSize").Get<long>();
        }
    }
}
