

using TinyMicroblog.Domain.Interfaces.Post;
using TinyMicroblog.Shared.Application.Interfaces;

namespace TinyMicroblog.Shared.Application.Services
{
    public class PostImageService : IPostImageService
    {

        private readonly IPostImageRepository _postImageRepository;

        public PostImageService(IPostImageRepository postImageRepository)
        {
            _postImageRepository = postImageRepository;
        }

        public async Task UpdatePostImage(int postId, string imageUrl, string imageType)
        {
            var postImages = await _postImageRepository.GetPostImagesByPostIdAsync(postId);
            if (postImages != null && postImages.Any() && postImages.Any(x => x.ImageType == imageType))
            {
                var postImage = postImages.FirstOrDefault(x => x.ImageType == imageType);
                postImage.ImageUrl = imageUrl;
                await _postImageRepository.UpdatePostImageAsync(postImage);

            }
            else
            {
                await _postImageRepository.CreatePostImageAsync(new Domain.Entities.PostImage
                {
                    PostId = postId,
                    ImageUrl = imageUrl,
                    ImageType = imageType
                });
            }
        }
    }
}
