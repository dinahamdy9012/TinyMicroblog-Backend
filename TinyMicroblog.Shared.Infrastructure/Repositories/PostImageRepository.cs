using Microsoft.EntityFrameworkCore;
using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Shared.Infrastructure.Data;

namespace TinyMicroblog.Shared.Infrastructure.Repositories
{
    public class PostImageRepository : Repository<TinyMicroblog.Domain.Entities.PostImage>, IPostImageRepository
    {
        private readonly TinyMicroblogDBContext _context;
        public PostImageRepository(TinyMicroblogDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CreatePostImageAsync(TinyMicroblog.Domain.Entities.PostImage postImage)
        {
            await _context.PostImages.AddAsync(postImage);
            await _context.SaveChangesAsync();
            return postImage.Id;
        }

        public async Task<IEnumerable<Domain.Entities.PostImage>> GetPostImagesByPostIdAsync(int postId)
        {
            return await _context.PostImages.AsNoTracking().Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<Domain.Entities.PostImage> UpdatePostImageAsync(Domain.Entities.PostImage postImage)
        {
            _context.PostImages.Attach(postImage);
            _context.PostImages.Update(postImage);
            await _context.SaveChangesAsync();
            return postImage;
        }

    }
}
