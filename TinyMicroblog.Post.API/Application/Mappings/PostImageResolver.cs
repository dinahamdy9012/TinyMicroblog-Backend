using AutoMapper;
using TinyMicroblog.Post.API.Application.Interfaces;
using TinyMicroblog.Post.API.Application.Models;

namespace TinyMicroblog.Post.API.Application.Mappings
{
    public class PostImageResolver : IValueResolver<Domain.Entities.Post, PostModel, string?>
    {
        IImageService _imageService { get; }

        public PostImageResolver(IImageService imageService)
        {
            _imageService = imageService;
        }

        public string? Resolve(Domain.Entities.Post source, PostModel destination, string? destMember, ResolutionContext context)
        {
            if (source.PostImages != null && source.PostImages.Any() && context.Items.ContainsKey("ScreenSize")) {

                return _imageService.GetBestImageUrl(source.PostImages, (int)context.Items["ScreenSize"]);
                
            }
            return null;
        }
    }
}
