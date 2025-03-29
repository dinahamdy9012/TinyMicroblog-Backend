namespace UploadService.Models
{
    public class DeleteFileRequestModel
    {
        public string FileName { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
    }
}
