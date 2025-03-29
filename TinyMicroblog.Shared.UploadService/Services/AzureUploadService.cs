using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.Net;
using TinyMicroblog.Servicebus.Interfaces;
using TinyMicroblog.Shared.UploadService.ErrorCodes;
using TinyMicroblog.Shared.UploadService.Interfaces;
using TinyMicroblog.Shared.UploadService.Models;
using TinyMicroblog.SharedKernel.Enums;
using TinyMicroblog.SharedKernel.Events;
using TinyMicroblog.SharedKernel.Exceptions;



namespace TinyMicroblog.Shared.UploadService.Services
{
    public class AzureUploadService : IUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceBusService _serviceBusService;
        public AzureUploadService(IConfiguration configuration, IServiceBusService serviceBusService)
        {
            _configuration = configuration;
            _serviceBusService = serviceBusService;
        }

        public async Task<string> UploadFile(UploadFileRequestModel uploadFileRequestModel)
        {
            if (uploadFileRequestModel is not UploadFileToAzureRequestModel uploadFileToAzureRequestModel)
                throw new ArgumentException("Invalid request type for AzureUpload.");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(uploadFileToAzureRequestModel.ContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(uploadFileToAzureRequestModel.FileName);
            if (blobClient == null)
            {
                throw new AppException(HttpStatusCode.InternalServerError, UploadErrorCodes.GetBlobClientException.ToString());
            }
            try
            {
                Stream file = new MemoryStream(uploadFileToAzureRequestModel.FileContent);
                var response = await blobClient.UploadAsync(file);
                if (response.GetRawResponse().Status == 201) // HTTP 201 Created
                {
                    var url = $"{_configuration["AzureStorageURL"]}{uploadFileToAzureRequestModel.ContainerName}/{uploadFileToAzureRequestModel.FileName}";
                    if(uploadFileToAzureRequestModel.EntityType == nameof(EntityTypeEnum.Post))
                    {
                        await _serviceBusService.SendNotification(_configuration["AzureServicebusQueues:UpdatePostQueue"], new FileUploadedEvent
                        {
                            EntityId = uploadFileToAzureRequestModel.EntityId,
                            EntityType = uploadFileToAzureRequestModel.EntityType,
                            FileUrl = url,
                            FileType = uploadFileToAzureRequestModel.FileType,
                        });
                    }

                    return url;
                }
                else
                {
                    throw new AppException(HttpStatusCode.InternalServerError, UploadErrorCodes.UploadFileFailed.ToString());
                }
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new AppException(HttpStatusCode.InternalServerError, UploadErrorCodes.UploadFileFailed.ToString());
            }
        }

        public async Task<string> ReplaceUploadedFile(UploadFileRequestModel uploadFileRequestModel)
        {
            if (uploadFileRequestModel is not UploadFileToAzureRequestModel uploadFileToAzureRequestModel)
                throw new ArgumentException("Invalid request type for AzureUpload.");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);

            var tempContainer = blobServiceClient.GetBlobContainerClient(uploadFileToAzureRequestModel.ContainerName);
            var blobClient = tempContainer.GetBlobClient(uploadFileToAzureRequestModel.FileName);
            Stream file = new MemoryStream(uploadFileToAzureRequestModel.FileContent);
            var response = await blobClient.UploadAsync(file, overwrite: true);

            if (response.GetRawResponse().Status == 201) // HTTP 201 Created
            {
                return $"{_configuration["AzureStorageURL"]}{uploadFileToAzureRequestModel.ContainerName}/{uploadFileToAzureRequestModel.FileName}";
            }
            else
            {
                throw new AppException(HttpStatusCode.InternalServerError, UploadErrorCodes.UploadFileFailed.ToString());
            }
        }

        public async Task<int> DeleteFile(DeleteFileRequestModel deleteFileRequestModel)
        {
            if (deleteFileRequestModel is not DeleteFileFromAzureRequestModel deleteFileFromAzureRequestModel)
                throw new ArgumentException("Invalid request type for AzureDelete.");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(deleteFileFromAzureRequestModel.ContainerName);
            var response = await cont.GetBlobClient(deleteFileFromAzureRequestModel.FileName).DeleteIfExistsAsync();
            return response.GetRawResponse().Status;
        }

        //public async Task<string> MoveFile(MoveFileRequestModel moveFileRequestModel)
        //{
        //    if (moveFileRequestModel is not MoveAzureFileRequestModel moveAzureFileRequestModel)
        //        throw new ArgumentException("Invalid request type for AzureUpload.");

        //    BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);

        //    var tempContainer = blobServiceClient.GetBlobContainerClient(moveAzureFileRequestModel.FromContainer);
        //    var permContainer = blobServiceClient.GetBlobContainerClient(moveAzureFileRequestModel.ToContainer);

        //    var tempBlob = tempContainer.GetBlobClient(moveAzureFileRequestModel.FileName);
        //    var permBlob = tempContainer.GetBlobClient($"{Guid.NewGuid()}.webp");

        //    await permBlob.StartCopyFromUriAsync(tempBlob.Uri);
        //    await tempBlob.DeleteAsync();

        //    return permBlob.Uri.ToString();
        //}
    }
}
