namespace UploadService.Models
{
    public class UploadFileRequestModel
    {
        public string FileName { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
        public MemoryStream FileContent { get; set; } = null!;
    }
}
