using AutoMapper;
using TinyMicroblog.Post.API.Application.Models;

namespace TinyMicroblog.Post.API.Application.Mappings
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Domain.Entities.Post, PostModel>()
             .ForMember(x => x.Id, s => s.MapFrom(p => p.Id))
             .ForMember(x => x.UserId, s => s.MapFrom(p => p.UserId))
            .ForMember(x => x.Username, s => s.MapFrom(p => p.Username))
            .ForMember(x => x.PostText, s => s.MapFrom(p => p.PostText))
            .ForMember(x => x.ImageUrl, s => s.MapFrom<PostImageResolver>())
            .ForMember(x => x.CreatedAt, s => s.MapFrom(p => p.CreatedAt));
        }
    }
}