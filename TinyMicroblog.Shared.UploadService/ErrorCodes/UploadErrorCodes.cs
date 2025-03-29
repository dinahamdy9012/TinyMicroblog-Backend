namespace TinyMicroblog.Shared.UploadService.ErrorCodes
{
    public enum UploadErrorCodes
    {
        InvalidExtension,
        InvalidSize,
        GetBlobClientException,
        UploadFileFailed
    }
}
