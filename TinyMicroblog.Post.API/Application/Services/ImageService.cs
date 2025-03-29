using TinyMicroblog.Domain.Entities;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.SharedKernel.Constants;
using TinyMicroblog.SharedKernel.Enums;

namespace TinyMicroblog.Post.API.Application.Services
{
    public class ImageService : IImageService
    {
        public ImageService() { }

        public string GetBestImageUrl(IEnumerable<PostImage> postImages, int screenSize)
        {
            var bestSize = ImageResizeSizesConstant.IMAGE_RESIZE_SIZES.OrderBy(s => Math.Abs(s.Value - screenSize)).First();
            return postImages.FirstOrDefault(x => x.ImageType == bestSize.Key)?.ImageUrl ?? postImages.FirstOrDefault(x => x.ImageType == nameof(ImageTypeEnum.Original))!.ImageUrl;
        }
    }
}
