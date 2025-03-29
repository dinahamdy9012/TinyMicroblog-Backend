using Microsoft.EntityFrameworkCore;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Shared.Infrastructure.Data;
using TinyMicroblog.Shared.Infrastructure.Repositories;

namespace TinyMicroblog.Post.API.Infrastructure.Repositories
{
    public class PostRepository : Repository<TinyMicroblog.Domain.Entities.Post>, IPostRepository
    {
        private readonly TinyMicroblogDBContext _context;
        public PostRepository(TinyMicroblogDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TinyMicroblog.Domain.Entities.Post>> GetAllPostsAsync()
        {
            return await _context.Posts.AsNoTracking()
                       .OrderByDescending(p => p.CreatedAt)
                       .ToListAsync();
        }

        public async Task<(IEnumerable<TinyMicroblog.Domain.Entities.Post> Posts, int TotalCount)> GetPaginatedPostsAsync(int pageIndex, int pageSize, string? searchValue)
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
                       .Include(x=>x.PostImages)
                       .ToListAsync(), totalCount);
        }

        public async Task<int> CreatePostAsync(TinyMicroblog.Domain.Entities.Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post.Id;
        }

        public async Task<Domain.Entities.Post?> GetPostByIdAsync(int id)
        {
            return await _context.Posts.AsNoTracking().Include(x=>x.PostImages).FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
