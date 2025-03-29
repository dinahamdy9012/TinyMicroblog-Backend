namespace TinyMicroblog.Domain.Interfaces.Post
{
    public interface IPostRepository : IRepository<TinyMicroblog.Domain.Entities.Post>
    {
        Task<IEnumerable<TinyMicroblog.Domain.Entities.Post>> GetAllPostsAsync();
        Task<(IEnumerable<TinyMicroblog.Domain.Entities.Post> Posts, int TotalCount)> GetPaginatedPostsAsync(int pageIndex, int pageSize, string? searchValue);
        Task<int> CreatePostAsync(TinyMicroblog.Domain.Entities.Post post);

        Task<Entities.Post?> GetPostByIdAsync (int id);
    }
}
