using Microsoft.EntityFrameworkCore;
using TinyMicroblog.Domain.Data;
using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Infrastructure.Repositories;

namespace TinyMicroblog.Infrastructure.Post.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly TinyMicroblogDBContext _context;
        public PostRepository(TinyMicroblogDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.AsNoTracking()
                       .OrderByDescending(p => p.CreatedAt)
                       .ToListAsync();
        }

        public async Task<(IEnumerable<Post> Posts, int TotalCount)> GetPaginatedPostsAsync(int pageIndex, int pageSize, string? searchValue)
        {
            var query = _context.Posts.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.PostText.ToLower().Contains(searchValue.ToLower()));
            }
            int totalCount = await query.CountAsync();
            return (await query
                       .OrderByDescending(p => p.CreatedAt)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .Include(x => x.User)
                       .ToListAsync(), totalCount);
        }

        public async Task<int> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post.Id;
        }


    }
}
