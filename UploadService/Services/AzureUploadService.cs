using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using SharedKernel.Exceptions;
using System.Net;
using UploadService.Enums;
using UploadService.Interfaces;
using UploadService.Models;

namespace UploadService.Services
{
    public class AzureUploadService : IUploadImageService
    {
        private readonly IConfiguration _configuration;

        public AzureUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> UploadFile(UploadFileRequestModel uploadFileRequestModel)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(uploadFileRequestModel.ContainerName);
            Stream originalImage = new MemoryStream(uploadFileRequestModel.FileContent);
            BlobClient blobClient = containerClient.GetBlobClient(uploadFileRequestModel.FileName);
            if (blobClient == null)
            {
                throw new AppException(HttpStatusCode.InternalServerError, UploadFileErrorCodes.GetBlobClientException.ToString());
            }
            try
            {
                var uploadedFileContent = await blobClient.UploadAsync(originalImage);
                return uploadedFileContent.GetRawResponse().Status;
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new AppException(HttpStatusCode.InternalServerError, UploadFileErrorCodes.UploadFileFailed.ToString());
            }
        }

        public async Task<int> DeleteFile(DeleteFileRequestModel deleteFileRequestModel)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration["ConnectionStrings:AzureStorage"]);
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(deleteFileRequestModel.ContainerName);
            var response = await cont.GetBlobClient(deleteFileRequestModel.FileName).DeleteIfExistsAsync();
            return response.GetRawResponse().Status;
        }

        // Move Image from Temp to Permanent Storage
        public async Task<string> MoveFileToPermanentStorageAsync(string tempFileName, string tempContainer, string permenantContainer)
        {
            var tempContainerClient = new BlobContainerClient(_configuration["ConnectionStrings:AzureStorage"], tempContainer);
            var permanentContainerClient = new BlobContainerClient(_configuration["ConnectionStrings:AzureStorage"], permenantContainer);

            var tempBlob = tempContainerClient.GetBlobClient(tempFileName);
            var permanentBlob = permanentContainerClient.GetBlobClient(tempFileName);

            if (await tempBlob.ExistsAsync())
            {
                await permanentBlob.StartCopyFromUriAsync(tempBlob.Uri);
                await tempBlob.DeleteAsync();
                return permanentBlob.Uri.ToString();
            }

            throw new FileNotFoundException("Temporary file not found.");
        }
    }
}
