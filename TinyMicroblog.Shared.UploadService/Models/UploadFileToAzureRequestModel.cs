namespace TinyMicroblog.Shared.UploadService.Models
{
    public class UploadFileToAzureRequestModel : UploadFileRequestModel
    {
        public string ContainerName { get; set; } = null!;
    }
}
