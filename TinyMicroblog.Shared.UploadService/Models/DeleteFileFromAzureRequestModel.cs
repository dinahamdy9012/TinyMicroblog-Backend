namespace TinyMicroblog.Shared.UploadService.Models
{
    public class DeleteFileFromAzureRequestModel : DeleteFileRequestModel
    {
        public string ContainerName { get; set; } = null!;
    }
}
