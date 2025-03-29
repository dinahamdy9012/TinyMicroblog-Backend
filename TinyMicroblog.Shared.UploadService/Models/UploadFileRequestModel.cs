namespace TinyMicroblog.Shared.UploadService.Models
{
    public class UploadFileRequestModel
    {
        public string FileName { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
        public int EntityId { get; set; }
        public string EntityType { get; set; } = null!;

        public string? FileType { get; set; } = null!;
    }
}
