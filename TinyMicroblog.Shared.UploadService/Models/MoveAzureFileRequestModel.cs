namespace TinyMicroblog.Shared.UploadService.Models
{
    public class MoveAzureFileRequestModel : MoveFileRequestModel
    {
        public MemoryStream FileSteam { get; set; } = null!;
        public string FromContainer { get; set; } = null!;
        public string ToContainer { get; set; } = null!;

    }
}
