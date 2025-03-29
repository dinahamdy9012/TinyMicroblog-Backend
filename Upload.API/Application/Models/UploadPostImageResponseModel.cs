namespace Upload.API.Application.Models
{
    public class UploadPostImageResponseModel
    {
        public string FileId { get; }
        public string FileName { get; }
        public string ImageUrl { get; }
        public UploadPostImageResponseModel(string fileId, string fileName, string imageUrl)
        {
            FileId = fileId;
            FileName = fileName;
            ImageUrl = imageUrl;
        }
    }
}
