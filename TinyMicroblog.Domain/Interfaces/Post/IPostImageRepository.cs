namespace TinyMicroblog.Domain.Interfaces.Post
{
    public interface IPostImageRepository : IRepository<TinyMicroblog.Domain.Entities.PostImage>
    {
        Task<IEnumerable<Entities.PostImage>> GetPostImagesByPostIdAsync(int postId);
        Task<Entities.PostImage> UpdatePostImageAsync(Entities.PostImage postImage);
        Task<int> CreatePostImageAsync(TinyMicroblog.Domain.Entities.PostImage postImage);
    }
}
