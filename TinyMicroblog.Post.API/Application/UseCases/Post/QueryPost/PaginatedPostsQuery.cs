using MediatR;
using TinyMicroblog.Post.API.Application.Models;
using TinyMicroblog.Shared.Application.Models;

namespace TinyMicroblog.Post.API.Application.UseCases.Post
{
    public class PaginatedPostsQuery : IRequest<DataResponse<List<PostModel>>>
    {
        public FilterModel FilterModel { get; }
        public int ScreenSize { get; }
        public PaginatedPostsQuery(FilterModel filterModel, int screenSize)
        {
            FilterModel = filterModel;
            ScreenSize = screenSize;
        }
    }
}
