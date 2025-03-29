namespace TinyMicroblog.Shared.Application.Interfaces
{
    public interface IPostImageService
    {
        Task UpdatePostImage(int postId, string imageUrl, string imageType);
    }
}
