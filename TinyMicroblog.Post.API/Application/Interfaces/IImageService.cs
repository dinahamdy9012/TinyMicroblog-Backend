using TinyMicroblog.Domain.Entities;

namespace TinyMicroblog.Post.API.Application.Interfaces
{
    public interface IImageService
    {
        string GetBestImageUrl(IEnumerable<PostImage> postImages, int screenSize);
    }
}
